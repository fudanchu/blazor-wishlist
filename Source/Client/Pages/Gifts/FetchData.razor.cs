using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wishlist.Shared.Utility;
using Wishlist.Shared.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Wishlist.Client.Pages.Gifts
{
    public partial class FetchData
    {
		PaginatedList<GiftDTO> gifts { get; set; } = new();
		int pageSize { get; set; } = 5;
		string filterText = "";
		bool isWaitingOnDialogResponse = false;

		private readonly List<int> pageSizes = new List<int> { 5, 10, 25, 50, 100, 1000 };

		async void FilterGifts(KeyboardEventArgs e)
		{
			await GetGiftsForPageX(1); //reset page index when filtering
			Console.WriteLine("Filtering on [" + filterText + "], " + gifts.Items.Count + " items, pages [" + gifts.TotalPages + "]");
			StateHasChanged();
		}
		protected async Task AdjustPageSize(ChangeEventArgs e)
		{
			pageSize = int.Parse(e.Value.ToString());
			if (pageSize * gifts.PageIndex > gifts.Items.Count)
			{
				gifts.PageIndex = 1;    //reset page index if we'd go over the edge
			}
			await GetGiftsForPageX(gifts.PageIndex);
		}
		protected async Task GetGiftsForPageX(int pageNumber)
		{
			if (pageNumber <= 0) { pageNumber = 1; }

			gifts = await httpClient.GetFromJsonAsync<PaginatedList<GiftDTO>>($"api/gift/GetPaged/{pageNumber}/{pageSize}/{filterText}");
		}
		protected override async Task OnInitializedAsync()
		{
			try
			{
				await GetGiftsForPageX(1);
			}
			catch
			{
				//HACK: when timed out the user get fails but
				//on client side we don't seem to know session expired
				navigationManager.NavigateTo(Globals.LoginUrlSessionExpired);
			}
		}

		async Task Delete(IGift targetGift)
		{
			if (!isWaitingOnDialogResponse)
			{
				isWaitingOnDialogResponse = true;
				var isConfirmed = await dialogService.Confirm("",
					$"Delete {targetGift.Name}?",
					new ConfirmOptions() { OkButtonText = "DELETE", CancelButtonText = "Nevermind" });
				if (isConfirmed.HasValue)
				{
					isWaitingOnDialogResponse = false;
					if (isConfirmed.Value == true)
					{
						await httpClient.DeleteAsync($"api/gift/{targetGift.Id}");
						await OnInitializedAsync();
					}
				}
			}
		}
	}
}