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
			var item = e.SelectedItem as AppClass;
			if (item == null)
				return;
			Settings.CurrentApp = item.Id;
			await NavigationService.PopModalAsync();
		}
	}
}
