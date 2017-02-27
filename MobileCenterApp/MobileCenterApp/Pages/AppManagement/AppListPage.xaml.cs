using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class AppListPage : BasePage
	{
		public AppListPage()
		{
			InitializeComponent();
		}

		async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
			var item = e.SelectedItem as AppClass;
			if (item == null)
				return;
			Settings.CurrentAppId = item.Id;
			await NavigationService.PopModalAsync();
		}

		public async void OnDelete(object sender, EventArgs e)
		{
			var app = ((MenuItem)sender).CommandParameter as AppClass;
			var result = await DisplayAlert("Are you sure?",$"Deleting '{app.DisplayName}'", "Delete","Nevermind");
			if (result)
				(BindingContext as AppListViewModel).DeleteCommand.Execute(sender);
		}
	}
}
