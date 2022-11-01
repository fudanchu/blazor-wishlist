using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Wishlist.Server.Models;
using Wishlist.Shared.Models.Security;
using Wishlist.Shared.Models.User;
using Microsoft.Extensions.Logging;
using Wishlist.Shared.Extensions;

namespace Wishlist.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ILogger _logger;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.Select(
                u => new ApplicationUserLoginDTO()
                {
                    Value = u.UserName,
                    DisplayText = u.DisplayName()
                }).ToListAsync();

            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> SendPasswordLinkReset(ResetPassword resetPassword)
        {
            var userName = resetPassword.UserName;

            //TODO: consider rate-limiting this so repeated attempts cannot tax mailgun
            var person = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == userName);
            if (person == null)
            {
                return BadRequest("User not found!");
            }
            try
            {
                var guid = Guid.NewGuid();
                var timeStamp = DateTime.Now.SetKindUtc();

                person.PasswordResetCode = guid;
                person.PasswordResetTimestamp = timeStamp;
                await _userManager.UpdateAsync(person);

                var result = Notifier.UserPasswordReset(person, guid, timeStamp);
                if (result.IsSuccessful)
                {
                    return Ok();
                }
                else
                {
                    string sendMessageError = $"Failed to send email! {result.ErrorMessage}";
                    _logger.LogError(sendMessageError);
                    return Problem(sendMessageError);
                }
            }
            catch (Exception e)
            {
                string sendMessageError = $"Error sending email! {e.Message}";
                _logger.LogError(sendMessageError);
                return Problem(sendMessageError);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                var userCount = _userManager?.Users?.Count();
                return BadRequest($"User [{request.UserName}] does not exist among {userCount} users.");
            }
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
            {
                bool isResetPasscode = user.PasswordResetCode.ToString() == request.Password.Trim();
                bool isPasscodeFresh = DateTime.Compare(
                    DateTime.Now.SetKindUtc(),
                    user.PasswordResetTimestamp.AddMinutes(
                        LoginRequest.MinutesBeforeTempPasswordExpires)) <= 0;
                if (isResetPasscode)
                {
                    if (!isPasscodeFresh)
                    {
                        return BadRequest("Passcode expired!");
                    }
                }
                else
                {
                    return BadRequest("Invalid credentials!");
                }
            }
            await _signInManager.SignInAsync(user, request.RememberMe);
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = Policies.CanImpersonate)]
        public async Task<IActionResult> Impersonate(ApplicationUserImpersonateDTO request)
        {
            var userToImpersonate = await _userManager.FindByIdAsync(request.Id);
            if (userToImpersonate == null)
            {
                return BadRequest("Could not find user to impersonate.");
            }
            var currentUserId = User.GetUserId();
            if (userToImpersonate.Id == currentUserId)
            {
                return Ok("Cannot impersonate yourself, nothing to do.");
            }
            await _signInManager.SignInAsync(userToImpersonate, false);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest parameters)
        {
            if (string.IsNullOrEmpty(parameters.UserAuthorizationCode) ||
                string.IsNullOrEmpty(parameters.AdminAuthorizationCode))
            {
                return BadRequest("System authorization codes are not configured!");
            }
            if (parameters.AuthorizationCode != parameters.UserAuthorizationCode &&
                parameters.AuthorizationCode != parameters.AdminAuthorizationCode)
            {
                return BadRequest("Wrong authorization code!");
            }
            var user = new ApplicationUser
            {
                LastName = parameters.LastName,
                FirstName = parameters.FirstName,
                NickName = parameters.NickName,
                Email = parameters.Email,
                ClearPassword = parameters.Password,
                IsAdmin = parameters.AuthorizationCode == parameters.AdminAuthorizationCode,
                GroupId = 0
            };
            await EnsureRolesExist();

            user.UserName = await UserHelper.GenerateUniqueUsername(user, _userManager);

            var result = await UserHelper.AddUser(user, _userManager);
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                return BadRequest(result.ErrorMessage);
            }

            return await Login(new LoginRequest
            {
                UserName = user.UserName,
                Password = user.ClearPassword
            });
        }

        private async Task EnsureRolesExist()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin, NormalizedName = Roles.Admin.ToUpper() });
            }
            if (!await _roleManager.RoleExistsAsync(Roles.Basic))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Basic, NormalizedName = Roles.Basic.ToUpper() });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        [HttpGet]
        public CurrentUser CurrentUserInfo()
        {
            return new CurrentUser
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                //UserName = User.Identity.Name,
                Claims = User.Claims
                .Select(c => new KeyValuePair<string, string>(c.Type, c.Value)).ToList()
            };
        }
    }
}