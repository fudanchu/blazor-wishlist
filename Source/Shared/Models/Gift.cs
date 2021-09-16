using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Wishlist.Shared.Models.User;
using Wishlist.Shared.Utility;

namespace Wishlist.Shared.Models
{
    public class GiftDTO : IGift
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [UrlValidation(ErrorMessage = "Links should start with http or https.")]
        public string WebLink { get; set; }
        [Column(TypeName = "Money")]
        public decimal? Cost { get; set; }
        [NotMapped]
        public string DisplayCost
        {
            get
            {
                if (Cost == null) return "";

                var formatString = Decimal.Truncate(Cost.Value) == Cost ? "C0" : "C";
                return (Cost?.ToString(formatString, CultureInfo.CurrentCulture));
            }
        }
        public int Rank { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public string UserAskingId { get; set; }
        public string UserBuyingId { get; set; }

        //TODO: enable other users to add/"suggest" items on a list?
        //where these items remain hidden from the actual individual
        public string UserSuggestingId { get; set; }
        [NotMapped]
        public string UserBuyingDisplayName { get; set; }
        [NotMapped]
        public string UserAskingDisplayName { get; set; }
        [NotMapped]
        public string UserSuggestingDisplayName { get; set; }
        public DateTime TimeBought { get; set; }
        public DateTime TimeAdded { get; set; }
    }

    public class Gift : IGift
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [UrlValidation(ErrorMessage = "Links should start with http or https.")]
        public string WebLink { get; set; }
        [Column(TypeName = "Money")]
        public decimal? Cost { get; set; }
        [NotMapped]
        public string DisplayCost
        {
            get
            {
                if (Cost == null) return "";

                var formatString = Decimal.Truncate(Cost.Value) == Cost ? "C0" : "C";
                return (Cost?.ToString(formatString, CultureInfo.CurrentCulture));
            }
        }
        public int Rank { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public string UserAskingId { get; set; }
        public ApplicationUser UserAsking { get; set; }
        [ForeignKey("UserId")]
        public string UserBuyingId { get; set; }

        #nullable enable
        public ApplicationUser? UserBuying { get; set; }
        #nullable disable
        [NotMapped]
        public string UserBuyingDisplayName { get; set; }
        [NotMapped]
        public string UserAskingDisplayName { get; set; }
        [NotMapped]
        public string UserSuggestingDisplayName { get; set; }
        //TODO: enable other users to add/"suggest" items on a list?
        //where these items remain hidden from the actual individual
        public string UserSuggestingId { get; set; }
        public DateTime TimeBought { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}