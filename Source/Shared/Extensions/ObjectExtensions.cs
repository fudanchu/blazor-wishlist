using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;
using System.ComponentModel.DataAnnotations;
using Wishlist.Shared.Models.User;
using AutoMapper;
using Wishlist.Shared.Utility;
using Wishlist.Shared.Models;

namespace Wishlist.Shared.Extensions
{
    public static class ObjectExtensions
    {
        static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static string ToDelimitedString(this List<string> list) =>
            string.Join(",", list);
        public static List<string> ToNewList(this string listAsString) =>
            listAsString.Split(new[] { ',' }).ToList<string>();

        private static Mapper GiftDtoMapper()
        {
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Gift, GiftDTO>()
            );
            return new Mapper(config);
        }
        private static Mapper GiftMapper()
        {
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<GiftDTO, Gift>()
            );
            return new Mapper(config);
        }
        private static Mapper ApplicationUserDtoMapper() {             
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<ApplicationUser, ApplicationUserDTO>()
            );
            return new Mapper(config);
        }

        public static GiftDTO ToDTO(this Gift gift)
        {
            var mapper = GiftDtoMapper();
            return mapper.Map<Gift, GiftDTO>(gift);
        }
        public static Gift FromDTO(this GiftDTO gift)
        {
            var mapper = GiftMapper();
            return mapper.Map<GiftDTO, Gift>(gift);
        }
        public static List<GiftDTO> ToDTO(this List<Gift> gifts)
        {
            var mapper = GiftDtoMapper();
            foreach (var gift in gifts)
            {
                gift.UserAskingDisplayName = gift.UserAsking?.DisplayName();
                gift.UserBuyingDisplayName = gift.UserBuying?.DisplayName();
            }
            return mapper.Map<List<Gift>, List<GiftDTO>>(gifts);
        }

        //TODO: merge with paginated list conversion?
        public static ApplicationUserDTO ToDTO(this ApplicationUser person)
        {
            var mapper = ApplicationUserDtoMapper();
            return mapper.Map<ApplicationUser, ApplicationUserDTO>(person);
        }
        public static List<ApplicationUserDTO> ToDTO(this List<ApplicationUser> people)
		{
            var mapper = ApplicationUserDtoMapper();
            return mapper.Map<List<ApplicationUser>, List<ApplicationUserDTO>>(people);
        }
        public static PaginatedList<ApplicationUserDTO> ToDTO(this PaginatedList<ApplicationUser> people)
        {
            //TODO: can a Class<T> be mapped?  Didn't seem to like it so semi-manual mapping for now...
            var mapper = ApplicationUserDtoMapper();
            var paginatedUserDTO = new PaginatedList<ApplicationUserDTO>
            {
                Items = mapper.Map<List<ApplicationUser>, List<ApplicationUserDTO>>(people.Items),

                HasNextPage = people.HasNextPage,
                HasPreviousPage = people.HasPreviousPage,
                PageIndex = people.PageIndex,
                TotalPages = people.TotalPages
            };

            return paginatedUserDTO;
        }

        #nullable enable
        public static string? ToJson(this object value) =>
            value is null ? null : Serialize(value, _options);

        public static T? FromJson<T>(this string? json) =>
            string.IsNullOrWhiteSpace(json) ? default : Deserialize<T>(json, _options);
        #nullable disable
    }
}