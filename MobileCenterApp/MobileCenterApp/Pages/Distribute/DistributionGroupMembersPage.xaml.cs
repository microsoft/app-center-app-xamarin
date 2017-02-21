using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class DistributionGroupMembersPage : BasePage
	{
		public DistributionGroupMembersPage()
		{
			InitializeComponent();
		}
		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as Tester;
			if (item == null)
				return;
			//(BindingContext as DistributeViewModel).OnSelected(item);
		}
		public async void OnDelete(object sender, EventArgs e)
		{
			var app = ((MenuItem)sender).CommandParameter as Tester;
			//var result = await DisplayAlert("Are you sure?", $"Deleting '{app.DisplayName}'", "Delete", "Nevermind");
			//if (result)
			//	(BindingContext as AppListViewModel).DeleteCommand.Execute(sender);
		}
	}
}
