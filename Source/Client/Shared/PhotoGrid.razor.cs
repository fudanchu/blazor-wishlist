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
            foreach (var person in Users)
            {
                person.ActiveClass = "inactive-user";
                if (person.Id == ActiveUserId)
                {
                    person.ActiveClass = "active-user";
                }
                if (OnlineUsers.Contains(person.Id))
                {
                    person.ActiveClass += " online-now";
                }
            }
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
            await OnInitializedAsync();
        }
        private async Task UpdateActiveUserId(string userId)
        {
            ActiveUserId = userId;
            UpdateUserImageClass();
            await ActiveUserIdChanged.InvokeAsync(ActiveUserId);
        }
    }
}
