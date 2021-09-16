using System;

namespace Wishlist.Shared.Models.User
{
    public interface IApplicationUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public DateTime LastListUpdate { get; set; }
        public string ListNote { get; set; }

        public string PictureType { get; set; }
        public string PictureData { get; set; }
        public string UserName { get; set; }

        public string ActiveClass { get; set; }
        public int ListCount { get; set; }
        public bool IsAdmin { get; set; }
        public int GroupId { get; set; }
    }
}
