using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wishlist.Shared.Models.User;
using Wishlist.Shared.Utility;

namespace Wishlist.Client.Pages.User
{
    public partial class FetchData
    {
        PaginatedList<ApplicationUserDTO> users { get; set; } = new();
        int pageSize { get; set; } = 5;
        string filterText = "";
        bool isWaitingOnDialogResponse = false;

        private readonly List<int> pageSizes = new List<int> { 5, 10, 25, 50, 100, 1000 };

        async void FilterUsers(KeyboardEventArgs e)
        {
            await GetUsersForPageX(1); //reset page index when filtering
            Console.WriteLine("Filtering on [" + filterText + "], " + users.Items.Count + " users, pages [" + users.TotalPages + "]");
            StateHasChanged();
        }
        protected async Task AdjustPageSize(ChangeEventArgs e)
        {
            pageSize = int.Parse(e.Value.ToString());
            if (pageSize * users.PageIndex > users.Items.Count)
            {
                users.PageIndex = 1;    //reset page index if we'd go over the edge
            }
            await GetUsersForPageX(users.PageIndex);
        }
        protected async Task GetUsersForPageX(int pageNumber)
        {
            if (pageNumber <= 0) { pageNumber = 1; }

            users = await httpClient.GetFromJsonAsync<PaginatedList<ApplicationUserDTO>>($"api/user/GetPaged/{pageNumber}/{pageSize}/{filterText}");
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUsersForPageX(1);
            }
            catch
            {
                //HACK: when timed out the user get fails but
                //on client side we don't seem to know session expired
                navigationManager.NavigateTo(Globals.LoginUrlSessionExpired);
            }
        }

        async Task Impersonate(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                toastService.ShowError("User has no id!", "Cannot Impersonate");
            }
            else
            {
                var userToImpersonate = new ApplicationUserImpersonateDTO { Id = userId };
                var response = await httpClient.PostAsJsonAsync($"api/auth/impersonate", userToImpersonate);
                if (response.IsSuccessStatusCode)
                {
                    navigationManager.NavigateTo("/", forceLoad: true);
                }
                else
                {
                    toastService.ShowError(response.ReasonPhrase);
                }
            }
        }
        async Task Delete(ApplicationUserDTO person)
        {
            if (!isWaitingOnDialogResponse)
            {
                isWaitingOnDialogResponse = true;
                var isConfirmed = await dialogService.Confirm("",
                    $"Delete {person.DisplayFullName()}?",
                    new ConfirmOptions() { OkButtonText = "DELETE", CancelButtonText = "Nevermind" });
                if (isConfirmed.HasValue)
                {
                    isWaitingOnDialogResponse = false;
                    if (isConfirmed.Value == true)
                    {
                        var response = await httpClient.DeleteAsync($"api/user/{person.Id}");
                        if (response.IsSuccessStatusCode)
                        {
                            await OnInitializedAsync();
                            toastService.ShowSuccess("Deleted");
                        }
                        else
                        {
                            toastService.ShowError(response.ReasonPhrase);
                        }
                    }
                }
            }
        }
    }
}