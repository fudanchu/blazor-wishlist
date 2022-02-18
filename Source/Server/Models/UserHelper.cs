using Microsoft.AspNetCore.Identity;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wishlist.Shared.Models.Security;
using Wishlist.Shared.Models.User;

namespace Wishlist.Server.Models
{
    public static class UserHelper
    {
        public static async Task<IRestResponse> AddUser(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserName))
                {
                    user.UserName = await GenerateUniqueUsername(user, userManager);
                }

                var result = await userManager.CreateAsync(user, user.ClearPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, IdentityRoles.Basic);
                    if (user.IsAdmin)
                    {
                        await userManager.AddToRoleAsync(user, IdentityRoles.Admin);
                    }

                    var notificationResult = Notifier.NewUserAdded(user);
                    return notificationResult;
                }
                else
                {
                    return new RestResponse() { ErrorMessage = result.Errors.FirstOrDefault()?.Description };
                }
            }
            catch (Exception ex)
            {
                return new RestResponse() { ErrorException = ex, ErrorMessage = ex.Message };
            }
        }

        public static async Task<string> GenerateUniqueUsername(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                user.LastName = "X";    //HACK: last name should never be empty but just in case
            }
            string uniqueUsername = (user.FirstName + user.LastName.Substring(0, 1)).Replace(" ", "");
            Random random = new();
            while (await userManager.FindByNameAsync(uniqueUsername) != null)
            {
                uniqueUsername += random.Next(1, 9).ToString();
            }
            return uniqueUsername;
        }
    }
}
