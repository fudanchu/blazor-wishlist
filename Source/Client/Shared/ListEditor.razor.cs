using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wishlist.Shared.Models;
using Wishlist.Shared.Utility;

namespace Wishlist.Client.Shared
{
    public partial class ListEditor
    {
        [Parameter]
        public string UserId { get; set; }
        [Parameter]
        public string ListNote { get; set; }
        [Parameter]
        public HubConnection UniversalHub { get; set; }

        public int giftId { get; set; }
        Gift gift = new Gift();
        Gift[] gifts { get; set; }
        private string buttonText = "Add";
        private string buttonClass = "btn-success";
        bool isWaitingOnDialogResponse = false;

        private bool isConnected =>
            UniversalHub.State == HubConnectionState.Connected;

        private async Task GetCurrentGifts()
        {
            gifts = await httpClient.GetFromJsonAsync<Gift[]>($"api/Gift/{UserId}");
        }

        protected override async Task OnInitializedAsync()
        {
            gift.UserAskingId = UserId;
            await GetCurrentGifts();
        }
        async Task NotifyOthersListChanged()
        {
            if (isConnected)
            {
                await UniversalHub.SendAsync("ListChanged", UserId);
            }
        }

        private void EnterEditState(Gift targetGift)
        {
            buttonText = "Edit";
            buttonClass = "btn-primary";

            gift.Id = targetGift.Id;    //the key is to set the id for editing!
            gift.Name = targetGift.Name;
            gift.Description = targetGift.Description;
            gift.WebLink = targetGift.WebLink;
            gift.Cost = targetGift.Cost;
        }
        private void EnterAddState()
        {
            buttonText = "Add";
            buttonClass = "btn-success";

            gift.Id = 0;
            gift.Name = "";
            gift.Description = "";
            gift.WebLink = "";
            gift.Cost = null;
        }

        private void RefreshParent()
        {
            //call parent to update accordingly
            commonService.CallAsyncRequestRefresh();
        }

        async Task Delete(Gift targetGift)
        {
            if (!isWaitingOnDialogResponse)
            {
                isWaitingOnDialogResponse = true;
                var isConfirmed = await dialogService.Confirm("",
                    $"Delete {targetGift.Name}?",
                    new ConfirmOptions()
                    {
                        OkButtonText = "DELETE",
                        CancelButtonText = "Nevermind",
                        AutoFocusFirstElement = true
                    });
                if (isConfirmed.HasValue)
                {
                    isWaitingOnDialogResponse = false;
                    if (isConfirmed.Value == true)
                    {
                        await httpClient.DeleteAsync($"api/gift/{targetGift.Id}");
                        await NotifyOthersListChanged();
                        await GetCurrentGifts();
                        RefreshParent();
                    }
                }
            }
        }

        async Task ProcessGift()
        {
            if (gift.Id == 0)
            {
                await httpClient.PostAsJsonAsync("api/Gift", gift);
                await NotifyOthersListChanged();
                RefreshParent();
            }
            else
            {
                await httpClient.PutAsJsonAsync($"api/Gift/{gift.Id}", gift);
            }
            await GetCurrentGifts();

            //clear out form for a new entry...
            EnterAddState();
        }
    }
}
