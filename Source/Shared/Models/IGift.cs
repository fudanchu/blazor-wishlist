using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wishlist.Shared.Models
{
    public interface IGift
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebLink { get; set; }
        public decimal? Cost { get; set; }
        public string DisplayCost { get; }
        public int Rank { get; set; }
        public string UserAskingId { get; set; }
        public string UserBuyingId { get; set; }
        public string UserSuggestingId { get; set; }
        public string UserBuyingDisplayName { get; set; }
        public string UserAskingDisplayName { get; set; }
        public string UserSuggestingDisplayName { get; set; }
        public DateTime TimeBought { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}
