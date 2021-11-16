using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wishlist.Shared.Utility;

namespace Wishlist.Shared.Models.User
{
	public class ApplicationUserImpersonateDTO
    {
        public string Id { get; set; }
    }
    public class ApplicationUserLoginDTO
    {
        public string Value { get; set; }
        public string DisplayText { get; set; }
    }
    public class ApplicationUserDTO : IApplicationUser
    {
        [Required]
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public DateTime LastListUpdate { get; set; }
        public string ListNote { get; set; }

        public string PictureType { get; set; }
        [Display(Name = "Picture")]
        public string PictureData { get; set; }
        public string UserName { get; set; }

        [NotMapped]
        public string ActiveClass { get; set; }
        [NotMapped]
        public int ListCount { get; set; }
        [NotMapped]
        public bool IsAdmin { get; set; }
        [NotMapped]
        public string SantaFor { get; set; }

        public string SantaForUserName { get; set; }

        //TODO: Use this to group users or remove it if not needed
        public int GroupId { get; set; }
        public string DisplayFullName()
        {
            return Globals.DisplayFullName(FirstName, LastName);
        }

        public string DisplayName()
        {
            return Globals.DisplayName(FirstName, LastName, NickName);
        }

        public string RenderPicture(string filePath = "/images/AngryGift.png")
        {
            return Globals.RenderPicture(PictureType, PictureData, filePath);
        }
    }

    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        //public virtual ICollection<Gift> Gifts { get; set; }

        //check for 5 unique characters
        private const string PasswordRegexString =
            "([\\x21-\\x7F])[\\x21-\\x7F]*(?!\\1)([\\x21-\\x7F])[\\x21-\\x7F]*(?!\\1|\\2)([\\x21-\\x7F])[\\x21-\\x7F]*";
        [NotMapped]
        [StringLength(50, ErrorMessage = "Try a password between 8 and 50 characters in length.", MinimumLength = 8)]
        [RegularExpression(PasswordRegexString, ErrorMessage = "Try more unique characters!")]
        public string ClearPassword { get; set; }

        [NotMapped]
        public string ActiveClass { get; set; }
        [NotMapped]
        public int ListCount { get; set; }
        [NotMapped]
        public bool IsAdmin { get; set; }

        [NotMapped]
        public string SantaFor { get; set; }

        public string SantaForUserName { get; set; }

        //TODO: Use this to group users or remove it if not needed
        public int GroupId { get; set; }
        //HACK: Heroku won't pull a username properly when inherited so trying an override
        public override string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public DateTime LastListUpdate { get; set; }
        public string ListNote { get; set; }
        public string PictureType { get; set; }
        [Display(Name = "Picture")]
        public string PictureData { get; set; }

        public Guid PasswordResetCode { get; set; }
        public DateTime PasswordResetTimestamp { get; set; }

        public string DisplayFullName()
        {
            return Globals.DisplayFullName(FirstName, LastName);
        }

        public string DisplayName()
        {
            return Globals.DisplayName(FirstName, LastName, NickName);
        }

        public string RenderPicture(string filePath = "/images/AngryGift.png")
        {
            return Globals.RenderPicture(PictureType, PictureData, filePath);
        }
    }
}