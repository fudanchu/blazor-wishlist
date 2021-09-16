using System.ComponentModel.DataAnnotations;

namespace Wishlist.Shared.Models.User
{
    public class LoginRequest
    {
        public const int MinutesBeforeTempPasswordExpires = 60;

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ResetCode { get; set; }
        public bool RememberMe { get; set; }
    }
}