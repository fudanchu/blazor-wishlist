using System.Collections.Generic;

namespace Wishlist.Shared.Models.User
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public List<KeyValuePair<string, string>> Claims { get; set; }
    }
}