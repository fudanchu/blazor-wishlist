using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Wishlist.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Wishlist.Server.Models;
using System;
using Npgsql;
using Wishlist.Server.Hubs;
using Wishlist.Shared.Models.Security;
using Wishlist.Shared.Models.User;
using Wishlist.Shared.Utility;

namespace Wishlist.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //hub added per https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-blazor?view=aspnetcore-5.0&tabs=visual-studio&pivots=webassembly
            services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddAuthorization(Policies.ConfigurePolicies());

            services.AddDbContext<ApplicationDBContext>(
                options => options.UseNpgsql(
                    #if DEBUG
                    Configuration.GetConnectionString("DefaultConnection")
                    #else
                    GetConnectionString("DATABASE_URL")
                    #endif
                )
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 3;
                    options.Lockout.MaxFailedAccessAttempts = 25;
                    options.Lockout.AllowedForNewUsers = true;
                }).
                AddEntityFrameworkStores<ApplicationDBContext>().
                AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = false;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                //options.Events.OnRedirectToLogin = context =>
                //{
                //    context.Response.Redirect(Globals.LoginUrlSessionExpired);
                //    return Task.CompletedTask;
                //};
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<WishlistHub>(Globals.HubPath);
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private string GetConnectionString(string envVarName)
        {
            var herokuConnectionString = Configuration[envVarName];
            if (string.IsNullOrEmpty(herokuConnectionString))
            {
                throw new Exception($"Could not find the environment variable {envVarName} for the connection string.");
            }

            var databaseUri = new Uri(herokuConnectionString);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }
    }
}
