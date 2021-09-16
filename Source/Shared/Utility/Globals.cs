using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wishlist.Shared.Utility
{
    public static class Globals
    {
        public static string LoginUrl = "login";
        public static string LoginUrlSessionExpired = "login/expired";
        public static string HubPath = "/wishlisthub";

        public static string DisplayFullName(string FirstName, string LastName)
        {
            return FirstName + " " + LastName;
        }

        public static string DisplayName(string FirstName, string LastName, string NickName)
        {
            string firstPart = string.IsNullOrWhiteSpace(NickName) ||
                    NickName.Length > FirstName.Length ? FirstName : NickName;
            if (string.IsNullOrEmpty(LastName))
            {
                return firstPart;
            }
            else
            {
                return string.Concat(firstPart, " ", LastName.AsSpan(0, 1));
            }
        }

        public static string RenderPicture(string PictureType, string PictureData,
            string filePath = "/images/AngryGift.png")
        {
            string imgSource = filePath;
            if (!string.IsNullOrWhiteSpace(PictureData))
            {
                imgSource = $"data:{PictureType};base64,{PictureData}";
            }
            return imgSource;
        }

    }
}
