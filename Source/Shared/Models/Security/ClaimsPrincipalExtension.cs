using System.Security.Claims;

namespace Wishlist.Shared.Models.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        }
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public static string DisplayFullName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue("FirstName") + " " + claimsPrincipal.FindFirstValue("LastName");
        }
        public static string DisplayFirstName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue("FirstName");
        }
    }
}