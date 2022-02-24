using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wishlist.Shared.Models.Security;
using Wishlist.Shared.Extensions;
using Wishlist.Shared.Utility;

namespace Wishlist.Client.Shared
{
    partial class ChatBox : IAsyncDisposable
    {
        [Parameter]
        public PhotoGrid PhotoGridComponent { get; set; }

        [Parameter]
        public EventCallback<string> OnlineUsersUpdated { get; set; }
        [Parameter]
        public HubConnection UniversalHub { get; set; }
        struct SpeechSynthesisVoice
        {
            public bool Default { get; set; }
            public string Lang { get; set; }
            public string Name { get; set; }

            public override string ToString() => Name;
        }

        private readonly List<double> _voiceSpeeds =
            Enumerable.Range(0, 12).Select(i => (i + 1) * .25).ToList();
        List<SpeechSynthesisVoice> _voices;
        string _voice = "Auto";
        double _voiceSpeed = 1;

        private readonly List<string> messages = new();

        private string messageInput = "";
        private string currentUserDisplayName;

        private bool isReadAloudMode = false;
        private bool isConnected =>
            UniversalHub.State == HubConnectionState.Connected;

        async Task SendMessage()
        {
            if (messageInput is { Length: > 0 })
            {
                await UniversalHub.SendAsync("SendMessage", currentUserDisplayName, messageInput);
                messageInput = "";

                await js.InvokeVoidAsync("app.updateScroll");
            }
        }
        async void OnEnterSubmit(KeyboardEventArgs e)
        {
            bool isEnterKeyPressed = e.Code == "Enter" || e.Code == "NumpadEnter";
            if (isConnected && isEnterKeyPressed)
            {
                await SendMessage();
            }
        }
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await UniversalHub.DisposeAsync();
        }
        private async Task AddMessageToChat(string message)
        {
            messages.Add(message);
            if (isReadAloudMode)
            {
                await js.InvokeVoidAsync("app.speak", message,
                    _voice, _voiceSpeed, "Google US English");
            }
            await js.InvokeVoidAsync("app.updateScroll");
            StateHasChanged();
        }
        private async Task ReadMessageAloud()
        {
            isReadAloudMode = !isReadAloudMode;
            if (isReadAloudMode)
            {
                await js.InvokeVoidAsync("app.speak", "Read-Aloud Mode Activated",
                    _voice, _voiceSpeed, "Google US English");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            UniversalHub.On<string>("ListChanged", async (userId) =>
            {
                //update the photo grid counts
                await commonService.CallAsyncRequestRefresh();
            });
            UniversalHub.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                await AddMessageToChat(encodedMsg);
            });

            //when users are arriving or leaving we update their active status
            UniversalHub.On<List<string>, string, string>("UpdateActivity",
                async (users, userWhoActed, userAction) =>
                {
                    string updateMessage = $"{userWhoActed} is {userAction}!";
                    bool isUpdateNewsworthy = messages == null || messages.Count == 0
                        || (!messages.Contains(updateMessage));

                    await OnlineUsersUpdated.InvokeAsync(users.ToDelimitedString());
                    await PhotoGridComponent.Refresh();

                    if (isUpdateNewsworthy)
                    {
                        await AddMessageToChat(updateMessage);
                    }
                });

            var authState = await authStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                await UniversalHub.StartAsync();
                currentUserDisplayName = authState.User.DisplayFirstName();
            }
            else
            {
                navigationManager.NavigateTo(Globals.LoginUrl);
            }
            await UpdateClientVoices(
                    await js.InvokeAsync<string>(
                    "app.getClientVoices", DotNetObjectReference.Create(this)));
        }

        [JSInvokable]
        public async Task UpdateClientVoices(string voicesJson) =>
            await InvokeAsync(() =>
            {
                var voices = voicesJson.FromJson<List<SpeechSynthesisVoice>>();
                if (voices is { Count: > 0 })
                {
                    _voices = voices;
                    StateHasChanged();
                }
            });
    }
}