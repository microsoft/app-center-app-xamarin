using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class DistributionGroupsPage : BasePage
	{
		public DistributionGroupsPage()
		{
			InitializeComponent();
		}
		public async void OnDelete(object sender, EventArgs e)
		{
			var app = ((MenuItem)sender).CommandParameter as DistributionGroup;
			var result = await DisplayAlert("Are you sure?", $"Deleting '{app.Name}'", "Delete", "Nevermind");
			if (result)
				(BindingContext as DistributionGroupsViewModel).DeleteCommand.Execute(sender);
		}
		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as DistributionGroup;
			if (item == null)
				return;
			(BindingContext as DistributionGroupsViewModel).OnSelected(item);
		}
	}
}
