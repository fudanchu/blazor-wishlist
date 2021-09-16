using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wishlist.Shared.Models;
using Wishlist.Shared.Models.User;

namespace Wishlist.Client.Shared
{
    public partial class ListViewer
    {
        [Parameter] public string CurrentListUserId { get; set; }
        [Parameter] public string LoggedInUserId { get; set; }

        [Parameter]
        public List<ApplicationUserDTO> Users { get; set; }

        List<GiftDTO> viewList { get; set; } = new List<GiftDTO>();

        ApplicationUserDTO activePerson { get; set; }
        bool loadFailed, isLoading, isViewingOwnList;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                loadFailed = false;
                isLoading = true;

                await UpdateActiveList();
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
            var isConfirmed = await dialogService.Confirm("So you bought this?",
                options: new ConfirmOptions() { OkButtonText = "YES", CancelButtonText = "Nevermind" });
            if (isConfirmed.HasValue && isConfirmed.Value == true)
            {
                gift.UserBuyingId = personId;
                try
                {
                    await httpClient.PutAsJsonAsync($"api/gift/checkoff", gift);
                }
                catch
                {
                    toastService.ShowError("Error on item check-off!", "UH OH");
                }
            }
        }

        async Task ReverseCheckOff(GiftDTO gift)
        {
            var isConfirmed = await dialogService.Confirm("You did NOT buy this?",
                options: new ConfirmOptions() { OkButtonText = "YES", CancelButtonText = "Nevermind" });
            if (isConfirmed.HasValue && isConfirmed.Value == true) 
            {
                try
                {
                    await httpClient.PutAsJsonAsync($"api/gift/reversecheckoff", gift);
                    gift.UserBuyingId = "";
                }
                catch
                {
                    toastService.ShowError("Error on item check-off!", "UH OH");
                }
            }
        }

        private async Task UpdateActiveList()
        {
            viewList = await httpClient.GetFromJsonAsync<List<GiftDTO>>($"api/gift/{CurrentListUserId}");
            try
            {
                activePerson = Users.Where(p => p.Id == CurrentListUserId).Single();
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
