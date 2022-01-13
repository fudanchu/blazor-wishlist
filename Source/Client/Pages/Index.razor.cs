using AutoMapper;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wishlist.Client.Shared;
using Wishlist.Shared.Extensions;
using Wishlist.Shared.Models.Security;
using Wishlist.Shared.Models.User;
using Wishlist.Shared.Utility;

namespace Wishlist.Client.Pages
{
    public partial class Index : IAsyncDisposable
    {
        private string ActiveUserId { get; set; }

        protected List<ApplicationUserDTO> Users { get; set; }
        protected ListViewer ListViewerComponent { get; set; }
        protected PhotoGrid PhotoGridComponent { get; set; }
        protected ChatBox ChatBoxComponent { get; set; }
        protected List<string> UserIds { get; set; } = new List<string>();
        protected HubConnection UniversalHub { get; set; }

        private async Task UserClicked(string newUserId)
        {
            ActiveUserId = newUserId;
            await ListViewerComponent.ChangeList(newUserId);
        }
        //HACK: since passing list<string> with callback is tricky doing a str conversion and back
        private void GetOnlineUsers(string users) =>
            UserIds = users.ToNewList();

        private async Task RefreshUserData()
        {
            Users = await client.GetFromJsonAsync<List<ApplicationUserDTO>>($"api/user/All/{ActiveUserId}");
            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            commonService.AsyncRefresh += RefreshPhotoGrid;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                UniversalHub = new HubConnectionBuilder()
                    .WithUrl(navigationManager.ToAbsoluteUri(Globals.HubPath))
                    .WithAutomaticReconnect()
                    .Build();

                ActiveUserId = authState.User.GetUserId();   //initialize with the current user's list showing

                try
                {
                    await RefreshUserData();
                }
                catch
                {
                    //HACK: when timed out the user get fails but
                    //on client side we don't seem to know session expired
                    navigationManager.NavigateTo(Globals.LoginUrlSessionExpired);
                }
            }
            else
            {
                navigationManager.NavigateTo(Globals.LoginUrl);
            }
        }
        private async Task RefreshPhotoGrid()
        {
            await RefreshUserData();
            await PhotoGridComponent.Refresh();
        }
        public async ValueTask DisposeAsync()
        {
            if (UniversalHub != null)
            {
                await UniversalHub.DisposeAsync();
            }
        }
    }
}