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
	}
}
