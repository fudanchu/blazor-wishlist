using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Learner.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int FamilyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string ShortName { get; set; }
        public DateTime LastListUpdate { get; set; }
        public string ListNote { get; set; }

        [FileValidation(new[] { ".png", ".jpg" })]
        public IBrowserFile Picture { get; set; }
        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
    public class FileValidationAttribute : ValidationAttribute
    {
        public FileValidationAttribute(string[] allowedExtensions)
        {
            AllowedExtensions = allowedExtensions;
        }

        private string[] AllowedExtensions { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (IBrowserFile)value;

            var extension = System.IO.Path.GetExtension(file.Name);

            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult($"File must have one of the following extensions: {string.Join(", ", AllowedExtensions)}.", new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
