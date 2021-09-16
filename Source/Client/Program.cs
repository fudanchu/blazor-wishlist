using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Wishlist.Client.Services;
using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Wishlist.Shared.Models.Security;
using Radzen;

namespace Wishlist.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(Policies.ConfigurePolicies());
            builder.Services.AddScoped<CustomStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICommonService, CommonService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            //For Radzen
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<TooltipService>();

            builder.Services.AddBlazoredToast();

            await builder.Build().RunAsync();
        }
    }
}
