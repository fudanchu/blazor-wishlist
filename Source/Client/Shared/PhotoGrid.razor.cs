using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wishlist.Shared.Models.User;

namespace Wishlist.Client.Shared
{
    public partial class PhotoGrid
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string ActiveUserId { get; set; }

        [Parameter]
        public EventCallback<string> ActiveUserIdChanged { get; set; }

        [Parameter]
        public List<ApplicationUserDTO> Users { get; set; }

        [Parameter]
        public List<string> OnlineUsers { get; set; }

        private void UpdateUserImageClass()
        {
            //Console.WriteLine("Calling UpdateUserImageClass with online users: " + String.Join(",",OnlineUsers));
            foreach (var person in Users)
            {
                person.ActiveClass = "inactive-user";
                if (person.Id == ActiveUserId)
                {
                    //Console.WriteLine($"setting active user class for {ActiveUserId}");
                    person.ActiveClass = "active-user";
                }
                if (OnlineUsers.Contains(person.Id))
                {
                    person.ActiveClass += " online-now";
                }
            }
            //Console.WriteLine("### looped through users and set inactive/ACTIVE class, StateHasChanged");
            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ActiveUserId))
            {
                await UpdateActiveUserId(Users.First().Id);
            }
            else
            {
                UpdateUserImageClass();
            }
        }
        public async Task Refresh()
        {
            //Console.WriteLine($"Refreshing photo grid from Index call...active: {ActiveUserId}");
            await OnInitializedAsync();
        }
        private async Task UpdateActiveUserId(string userId)
        {
            //Console.WriteLine($"User: {userId}");
            ActiveUserId = userId;
            UpdateUserImageClass();
            await ActiveUserIdChanged.InvokeAsync(ActiveUserId);
        }
    }
}
