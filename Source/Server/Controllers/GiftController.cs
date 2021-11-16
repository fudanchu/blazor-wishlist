using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wishlist.Server.Data;
using Wishlist.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Wishlist.Shared.Models.Security;
using Wishlist.Shared.Utility;
using System.Collections.Generic;
using Wishlist.Shared.Extensions;

namespace Wishlist.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public GiftController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("GetPaged/{pageNumber:int}/{pageSize:int}/{filter?}")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10, string filter = null)
        {
            if (filter != null) { filter = filter.ToLower(); }
            IQueryable<Gift> targetGifts = 
                    _context.Gifts.Include(g => g.UserAsking).Include(g => g.UserBuying);

            if (!string.IsNullOrWhiteSpace(filter)) {
                targetGifts = targetGifts.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLower().Contains(filter))
                    || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(filter)));
            }
            var selectGifts = await PaginatedList<Gift>.CreateAsync(
                targetGifts.OrderBy(g => g.Rank), pageNumber, pageSize);

            var selectGiftsDTO = new PaginatedList<GiftDTO>
            {
                HasNextPage = selectGifts.HasNextPage,
                HasPreviousPage = selectGifts.HasPreviousPage,
                PageIndex = selectGifts.PageIndex,
                TotalPages = selectGifts.TotalPages,
                Items = selectGifts.Items.ToDTO()
            };
            return Ok(selectGiftsDTO);
        }

        [HttpGet("{userId?}")]
        public async Task<IActionResult> Get(string userId = null)
        {
            var gifts = userId == null ?
                await _context.Gifts
                .Include(g => g.UserAsking)
                .Include(g => g.UserBuying)
                .OrderBy(g => g.Name).ToListAsync() :
                await _context.Gifts
                .Include(g => g.UserAsking)
                .Include(g => g.UserBuying)
                .Where(gift => gift.UserAskingId == userId)
                .OrderBy(g => g.Name).ToListAsync();

            return Ok(gifts.ToDTO());
        }

        // PUT: api/Gift/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(GiftDTO gift)
        {
            var currentUserId = User.GetUserId();
            var existingGift = await _context.Gifts.Where(g => g.Id == gift.Id).SingleAsync();
            //ensure gift exists
            if (existingGift == null)
            {
                return NotFound();
            }
            //ensure gift belongs to this user
            if (existingGift.UserAskingId != currentUserId)
            {
                return BadRequest();
            }
            //update gift
            existingGift.Cost = gift.Cost;
            existingGift.Description = gift.Description;
            existingGift.Name = gift.Name;
            existingGift.Rank = gift.Rank;
            existingGift.WebLink = gift.WebLink;

            var userAsking = await _context.People.FindAsync(gift.UserAskingId);
            userAsking.LastListUpdate = DateTime.Now;
            await _context.SaveChangesAsync();

            //_context.Entry(gift).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Gift
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> Post(GiftDTO gift)
        {
            bool isForAnotherUser = User.GetUserId() != gift.UserAskingId;
            bool isAdmin = User.IsInRole(Roles.Admin);
            if (isForAnotherUser && !isAdmin)
            {
                return BadRequest("Only admins can submit gifts for other users!");
            }

            gift.TimeAdded = DateTime.Now;
            _context.Gifts.Add(gift.FromDTO());

            var userAsking = await _context.People.FindAsync(gift.UserAskingId);
            userAsking.LastListUpdate = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(gift.Id);
        }

        // DELETE: api/Gift/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteGift(long id)
        {
            //ensure this gift exists
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
            {
                return NotFound();
            }
            //prevent this gift from being deleted by you if it's not yours
            //unless you're an admin user!
            var currentUserId = User.GetUserId();
            bool isYourGift = gift.UserAskingId == currentUserId;
            bool isAnAdminDeleting = User.IsInRole(Roles.Admin);
            bool invalidDeleteAttempt = !(isYourGift || isAnAdminDeleting);
            if (invalidDeleteAttempt)
            {
                return BadRequest("Only admins can delete the gifts of others!");
            }
            _context.Gifts.Remove(gift);

            var userAsking = await _context.People.FindAsync(gift.UserAskingId);
            userAsking.LastListUpdate = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("ReverseCheckoff")]
        public async Task<IActionResult> ReverseCheckoff(GiftDTO gift)
        {
            var currentUserId = User.GetUserId();
            var existingGift = await _context.Gifts.
                Where(g => g.Id == gift.Id).SingleAsync();

            //reverse the checkoff but only if YOU checked it out!
            bool isCheckedOutToYou = existingGift.UserBuyingId == currentUserId;
            if (isCheckedOutToYou)
            {
                existingGift.UserBuyingId = null;
                existingGift.TimeBought = DateTime.MinValue;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cannot put this back because it's not checked out to you!");
            }
            return Ok(existingGift);
        }

        [HttpPut("Checkoff")]
        public async Task<IActionResult> Checkoff(GiftDTO gift)
        {
            var currentUserId = User.GetUserId();
            var existingGift = await _context.Gifts.FindAsync(gift.Id);

            //don't allow checkoff unless it's appropriate
            bool isYourGift = existingGift.UserAskingId == currentUserId;
            bool isAvailableForCheckout = string.IsNullOrEmpty(existingGift.UserBuyingId);
            bool isCheckedOutToYou = existingGift.UserBuyingId == currentUserId;
            if (!isYourGift && (isAvailableForCheckout || isCheckedOutToYou))
            {
                existingGift.UserBuyingId = currentUserId;
                existingGift.TimeBought = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Someone checked it off first!");
            }
            return Ok(existingGift);
        }
    }
}