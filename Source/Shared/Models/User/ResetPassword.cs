using System;
using System.ComponentModel.DataAnnotations;

namespace Wishlist.Shared.Models.User
{
    public class ResetPassword
    {
        public static string MailDomainName() => Environment.GetEnvironmentVariable("WISHLIST_MAILGUN_DOMAIN");
        public static string MailApiKey() => Environment.GetEnvironmentVariable("WISHLIST_MAILGUN_API_KEY");
        public const string MailApiUrl = "https://api.mailgun.net/v3/";

        [Required]
        public string UserName { get; set; }
    }
}