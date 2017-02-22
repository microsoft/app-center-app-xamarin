using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class DistributionGroupReleasesPage : BasePage
	{
		public DistributionGroupReleasesPage()
		{
			InitializeComponent();
			Title = "Releases";
		}
		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as Release;
			if (item == null)
				return;
			(BindingContext as DistributionGroupDetailsViewModel).OnReleaseSelected(item);
		}
		public void OnDelete(object sender, EventArgs e)
		{
			var app = ((MenuItem)sender).CommandParameter as Release;
			//var result = await DisplayAlert("Are you sure?", $"Deleting '{app.DisplayName}'", "Delete", "Nevermind");
			//if (result)
			//	(BindingContext as AppListViewModel).DeleteCommand.Execute(sender);
			App.NotImplemented();
		}
		public async void OnInstall(object sender, EventArgs e)
		{
			var app = ((MenuItem)sender).CommandParameter as Release;
			await (App.Current as App).InstallRelease(app);
		}
	}
}
