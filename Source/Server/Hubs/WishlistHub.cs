using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Wishlist.Shared.Models.Security;

namespace Wishlist.Server.Hubs
{
    [Authorize]
    public class WishlistHub : Hub
    {
        private static readonly List<string> _onlineUsers = new();

        public List<string> WhoIsOnlineNow()
		{
            return _onlineUsers;
		}
        private string GetCurrentUserId()
        {
            return Context.User.GetUserId();
        }
        private string GetCurrentUserDisplayName()
        {
            return Context.User.DisplayFullName();
        }
        public override async Task OnConnectedAsync()
        {
            var thisUserId = GetCurrentUserId();
            if (!_onlineUsers.Contains(thisUserId))
            {
                _onlineUsers.Add(thisUserId);
            }
            await Clients.All.SendAsync("UpdateActivity", _onlineUsers,
                    GetCurrentUserDisplayName(), "here");
            await base.OnConnectedAsync();
        }
        private static bool MatchesUser(string userId, string thisUserId)
        {
            return userId.ToLower() == thisUserId.ToLower();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {

            var thisUserId = GetCurrentUserId();
            if (_onlineUsers.Contains(thisUserId))
            {
                _onlineUsers.RemoveAll(u=>MatchesUser(u,thisUserId));
            }
            await Clients.All.SendAsync("UpdateActivity", _onlineUsers,
                    GetCurrentUserDisplayName(), "leaving");
            await base.OnDisconnectedAsync(exception);
        }
        public async Task ListChanged(string userId)
        {
            await Clients.All.SendAsync("ListChanged", userId);
        }
        public async Task GiftCheckoffUpdate()
        {
            await Clients.All.SendAsync("GiftCheckoffUpdate");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
