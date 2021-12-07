using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wishlist.Shared.Models.User;

namespace Wishlist.Client.Pages.Authentication
{
    public partial class Login
    {
        private string problem;
        private bool loading, usersExist = true;

        [Parameter]
        public string isExpired { get; set; }

        LoginRequest loginRequest { get; set; } = new();
        ResetPassword resetPassword { get; set; } = new();
        List<ApplicationUserLoginDTO> Users { get; set; }

        bool isWaitingOnDialogResponse = false;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(isExpired))
            {
                toastService.ShowWarning("Your session timed out!", "SORRY...");
            }
            loading = true;
            try
            {
                Users = await httpClient.GetFromJsonAsync<List<ApplicationUserLoginDTO>>("api/auth/GetUsers");
            }
            catch (Exception ex)
            {
                problem = ex.Message;
            }
            finally
            {
                StateHasChanged();
                loading = false;
            }
        }
        private async Task SendPasswordLink()
        {
            if (!isWaitingOnDialogResponse)
            {
                isWaitingOnDialogResponse = true;
                var isConfirmed = await dialogService.Confirm("You want a password reset message sent to your email on file?",
                    $"Send password reset?",
                    new ConfirmOptions() { OkButtonText = "SEND", CancelButtonText = "Nevermind" });
                if (isConfirmed.HasValue)
                {
                    isWaitingOnDialogResponse = false;
                    if (isConfirmed.Value == true)
                    {
                        resetPassword.UserName = loginRequest.UserName;
                        var response = await httpClient.PostAsJsonAsync("api/auth/sendpasswordlinkreset", resetPassword);
                        if (response.IsSuccessStatusCode)
                        {
                            toastService.ShowSuccess("Maybe in your spam folder.", "Message sent to your email!");
                        }
                        else
                        {
                            toastService.ShowError(await response.Content.ReadAsStringAsync(), "Failed to send reminder!");
                        }
                    }
                }
            }
        }
        private async Task OnSubmit()
        {
            try
            {
                await authStateProvider.Login(loginRequest);
                navigationManager.NavigateTo("");
            }
            catch (Exception ex)
            {
                toastService.ShowError(ex.Message);
            }
        }
    }
}
