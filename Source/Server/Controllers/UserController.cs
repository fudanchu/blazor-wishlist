using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wishlist.Server.Data;
using Wishlist.Server.Hubs;
using Wishlist.Server.Models;
using Wishlist.Shared.Extensions;
using Wishlist.Shared.Models.Security;
using Wishlist.Shared.Models.User;
using Wishlist.Shared.Utility;

namespace Wishlist.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ILogger _logger;

        public UserController(UserManager<ApplicationUser> userManager,
            ApplicationDBContext context,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        private async Task<ApplicationUser> PopulatePersonData(ApplicationUser person)
        {
            person.ListCount = _context.Gifts.Where(g => g.UserAskingId == person.Id).Count();
            person.IsAdmin = await _userManager.IsInRoleAsync(person, Roles.Admin);
            return person;
        }
        private async Task<List<ApplicationUser>> PopulatePeopleData(List<ApplicationUser> people)
        {

            foreach (var person in people)
            {
                person.MapFromUser(await PopulatePersonData(person));
            }
            return people;
        }
        // GET: api/User/All/23984-2-323
        /// <summary>
        /// Gets all users, if an id is supplied order that user first
        /// </summary>
        [HttpGet("All/{id?}")]
        public async Task<IActionResult> All(string id = null)
        {
            List<ApplicationUser> people;
            WishlistHub hub = new();

            if (string.IsNullOrEmpty(id))
            {
                people = await _context.People.ToListAsync();
            }
            else
            {
                //order logged-in user first, then other active users, then by name
                people = await _context.People.OrderByDescending(p => p.Id == id)
                    .ThenByDescending(p => hub.WhoIsOnlineNow().Contains(p.Id))
                    .ThenBy(p => p.LastName).ThenBy(p => p.FirstName).ToListAsync();
            }
            people = await PopulatePeopleData(people);

            return Ok(people.ToDTO());
        }

        [HttpGet("GetPaged/{pageNumber:int}/{pageSize:int}/{filter?}")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10,
            string filter = null)
        {
            PaginatedList<ApplicationUser> people;
            IQueryable<ApplicationUser> targetPeople = _context.People;
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                targetPeople = targetPeople.Where(p =>
                    p.FirstName.ToLower().Contains(filter) ||
                    p.LastName.ToLower().Contains(filter) ||
                    p.UserName.ToLower().Contains(filter));
            }

            people = await PaginatedList<ApplicationUser>.CreateAsync(
                targetPeople.OrderBy(p => p.LastName).ThenBy(p => p.FirstName),
                pageNumber, pageSize);
            people.Items = await PopulatePeopleData(people.Items);

            return Ok(people.ToDTO());
        }

        private async Task<ApplicationUser> GetPersonById(string id)
        {
            var person = await _userManager.FindByIdAsync(id);
            if (person == null)
            {
                return null;
            }
            return await PopulatePersonData(person);
        }

        // GET: api/User/543-23-234
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var person = await GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/User/5123-213-1233
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> Put(ApplicationUser person)
        {
            string currentUserId = User.GetUserId();
            bool isEditingAnotherUser = currentUserId != person.Id;
            bool isCurrentUserAnAdmin = User.IsInRole(Roles.Admin);
            if (isEditingAnotherUser && !isCurrentUserAnAdmin)
            {
                return BadRequest("You cannot edit another user!");
            }

            var existingUser = await GetPersonById(person.Id);
            if (existingUser == null)
            {
                return BadRequest("User not found!");
            }

            if (person.IsAdmin && !isCurrentUserAnAdmin)
            {
                return BadRequest("You're not an admin so you cannot create admins!");
            }

            bool sendEmailAlert = existingUser.Email != person.Email;
            bool isNewAdminStatus = existingUser.IsAdmin != person.IsAdmin;
            bool isNewPassword = !string.IsNullOrEmpty(person.ClearPassword);

            existingUser.MapFromUser(person);

            if (isNewPassword)
            {
                //HACK: this may result in a user without a pwd if something goes wrong
                await _userManager.RemovePasswordAsync(existingUser);
                await _userManager.AddPasswordAsync(existingUser, person.ClearPassword);
            }
            if (isNewAdminStatus)
            {
                if (person.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(existingUser, Roles.Admin);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(existingUser, Roles.Admin);
                }
            }
            var userUpdate = await _userManager.UpdateAsync(existingUser);
            if (userUpdate.Succeeded)
            {
                if (sendEmailAlert)
                {
                    try
                    {
                        var emailResult = Notifier.UserEmailUpdated(existingUser);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unable to update user email!", ex.Message);
                    }
                }
                return Ok();
            }
            else
            {
                string errorList = string.Join(",", userUpdate.Errors.Select(e => e.Description).ToArray());
                return Problem(errorList);
            }
        }

        // POST: api/User
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> Post(ApplicationUser user)
        {
            user.UserName = await UserHelper.GenerateUniqueUsername(user, _userManager);

            var result = await UserHelper.AddUser(user, _userManager);
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(user.Id);
        }

        // DELETE: api/User/523423-234243343-43453
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId == id)
            {
                return BadRequest("You cannot delete yourself!!");
            }
            var currentUser = await _context.People.FindAsync(currentUserId);
            if (!(await _userManager.IsInRoleAsync(currentUser, Roles.Admin)))
            {
                return BadRequest("You don't have the power to delete a user!");
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            var thisPersonsGifts = _context.Gifts.Where(g => g.UserAskingId == person.Id);
            if (thisPersonsGifts != null && thisPersonsGifts.Any())
            {
                _context.Gifts.RemoveRange(thisPersonsGifts);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}