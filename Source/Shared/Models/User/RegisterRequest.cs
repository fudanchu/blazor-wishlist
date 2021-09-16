using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishlist.Shared.Models.User
{
    public class RegisterRequest
    {
        //These codes are manually set and sent to users whom we want to register
        public string UserAuthorizationCode = Environment.GetEnvironmentVariable("WISHLIST_USER_AUTH_CODE");
        public string AdminAuthorizationCode = Environment.GetEnvironmentVariable("WISHLIST_ADMIN_AUTH_CODE");

        [Required]
        [NotMapped]
        public string AuthorizationCode { get; set; }

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string NickName { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirm { get; set; }

        //TODO: remove or utilize if a user-grouped system is desired
        public int GroupId { get; set; }
    }
}