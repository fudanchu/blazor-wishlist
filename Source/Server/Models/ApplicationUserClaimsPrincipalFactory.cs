using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Wishlist.Shared.Models.User;

namespace Wishlist.Server.Models
{
    public class ApplicationUserClaimsPrincipalFactory :
                           UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public UserManager<ApplicationUser> MyUserManager { get; private set; }
        //public RoleManager<IdentityRole> myRoleManager { get; private set; }
        //public IdentityOptions myIdentityOptions { get; private set; }

        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
                                  IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
            //if (userManager == null)
            //{
            //    throw new ArgumentNullException(nameof(userManager));
            //}
            //if (roleManager == null)
            //{
            //    throw new ArgumentNullException(nameof(roleManager));
            //}
            //if (optionsAccessor == null || optionsAccessor.Value == null)
            //{
            //    throw new ArgumentNullException(nameof(optionsAccessor));
            //}
            //myUserManager = userManager;
        }

        protected override async Task<ClaimsIdentity>
                 GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity claims = await
                            base.GenerateClaimsAsync(user);

            claims.AddClaim(new Claim("GroupId", user.GroupId.ToString()));
            claims.AddClaim(new Claim("PictureData", user.PictureData ?? ""));
            claims.AddClaim(new Claim("NickName", user.NickName ?? ""));
            claims.AddClaim(new Claim("FirstName", user.FirstName ?? ""));
            claims.AddClaim(new Claim("LastName", user.LastName ?? ""));
            claims.AddClaim(new Claim("LastListUpdate", user.LastListUpdate.ToString()));

            return claims;
        }
    }
}