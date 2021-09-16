using Wishlist.Shared.Models;
using System.Threading.Tasks;
using Wishlist.Shared.Models.User;

namespace Wishlist.Client.Services
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
        //Task SendPasswordLinkReset(ForgotPassword forgotPassword);
        Task<CurrentUser> CurrentUserInfo();
    }
}
