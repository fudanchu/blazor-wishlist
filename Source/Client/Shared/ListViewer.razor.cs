using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wishlist.Shared.Models;
using Wishlist.Shared.Models.User;
using Wishlist.Shared.Utility;

namespace Wishlist.Client.Shared
{
    public partial class ListViewer
    {
        [Parameter] public string CurrentListUserId { get; set; }
        [Parameter] public string LoggedInUserId { get; set; }

        [Parameter]
        public List<ApplicationUserDTO> Users { get; set; }

        [Parameter]
        public HubConnection UniversalHub { get; set; }
        List<GiftDTO> viewList { get; set; } = new List<GiftDTO>();

        ApplicationUserDTO activePerson { get; set; }
        bool loadFailed, isLoading, isViewingOwnList, isWaitingOnDialogResponse = false;

        private bool isConnected =>
            UniversalHub.State == HubConnectionState.Connected;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                loadFailed = false;
                isLoading = true;

                await UpdateActiveList();

                UniversalHub.On("GiftCheckoffUpdate", async () =>
                {
                    await UpdateActiveList();
                });
                UniversalHub.On<string>("ListChanged", async (userId) =>
                {
                    if (userId == CurrentListUserId)
                    {
                        //update the viewed list to reflect any potential edit
                        await UpdateActiveList();
                    }
                });
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                loadFailed = true;
                Console.WriteLine($"Problem initializing list data! {e.Message}");
                toastService.ShowError("Error initializing list data!");
            }
            isLoading = false;
        }

        async Task CheckOff(GiftDTO gift, string personId)
        {
            if (!isWaitingOnDialogResponse)
            {
                isWaitingOnDialogResponse = true;
                var isConfirmed = await dialogService.Confirm("So you bought this?",
                    options: new ConfirmOptions() { OkButtonText = "YES", CancelButtonText = "Nevermind" });
                if (isConfirmed.HasValue)
                {
                    isWaitingOnDialogResponse = false;
                    if (isConfirmed.Value == true)
                    {
                        gift.UserBuyingId = personId;
                        try
                        {
                            await httpClient.PutAsJsonAsync($"api/gift/checkoff", gift);
                            await NotifyOthersListCheckedOff();
                        }
                        catch
                        {
                            toastService.ShowError("Error on item check-off!", "UH OH");
                        }
                    }
                }
            }
        }

        async Task NotifyOthersListCheckedOff()
        {
            if (isConnected)
            {
                await UniversalHub.SendAsync("GiftCheckoffUpdate");
            }
        }

        async Task ReverseCheckOff(GiftDTO gift)
        {
            if (!isWaitingOnDialogResponse)
            {
                isWaitingOnDialogResponse = true;
                var isConfirmed = await dialogService.Confirm("You did NOT buy this?",
                    options: new ConfirmOptions() { OkButtonText = "YES", CancelButtonText = "Nevermind" });
                if (isConfirmed.HasValue)
                {
                    isWaitingOnDialogResponse = false;
                    if (isConfirmed.Value == true)
                    {
                        try
                        {
                            await httpClient.PutAsJsonAsync($"api/gift/reversecheckoff", gift);
                            await NotifyOthersListCheckedOff();
                            gift.UserBuyingId = "";
                        }
                        catch
                        {
                            toastService.ShowError("Error on item check-off!", "UH OH");
                        }
                    }
                }
            }
        }

        private async Task UpdateActiveList()
        {
            viewList = await httpClient.GetFromJsonAsync<List<GiftDTO>>($"api/gift/{CurrentListUserId}");
            try
            {
                activePerson = Users.Single(p => p.Id == CurrentListUserId);
            }
            catch
            {
                throw new Exception("Unable to find this user! Maybe clear your browser cache and log in again?");
            }
            isViewingOwnList = activePerson.Id == LoggedInUserId;
            StateHasChanged();
        }

        public async Task ChangeList(string id)
        {
            CurrentListUserId = id;
            await UpdateActiveList();
        }
    }
}
