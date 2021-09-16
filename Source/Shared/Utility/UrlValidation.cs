using System;
using System.ComponentModel.DataAnnotations;

namespace Wishlist.Shared.Utility
{
    public class UrlValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            var lowerValue = value.ToString().ToLower();
            if (UrlValidation.IsValidUrl(lowerValue))
            {
                return null;
            }
            return new ValidationResult(ErrorMessage, 
                new[] { validationContext.MemberName });
        }
    }
    public static class UrlValidation
    {
        public static bool IsValidUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri validatedUri)) //.NET URI validation.
            {
                //If seems valid, check for the scheme too
                return (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
            }
            return false;
        }
    }
}