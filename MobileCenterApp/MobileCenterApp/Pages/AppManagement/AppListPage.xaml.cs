using System;
using System.Collections.Generic;
using MobileCenterApp.Models;
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

	    private void ViewCell_Appearing(object sender, EventArgs e)
	    {
	        var viewCell = sender as ViewCell;
	        var app = viewCell.BindingContext as AppClass;
	        var label = viewCell.FindByName<Label>("AppIconLabel");
	        if (viewCell != null && app != null && label != null)
	        {
	            var initial = app.DisplayName.Substring(0, 1);
	            var identicon = new Identicon(initial, 40);
	            label.TextColor = Color.FromHex(identicon.TextStyle["color"]);
	            label.Text = initial;
	            RoundedCornersEffect.SetCornerRadius(label, 3.0f);
	            RoundedCornersEffect.SetBackgroundColor(label, Color.FromHex(identicon.Style["background"]));
	            RoundedCornersEffect.SetHasRoundedCorners(label, true); //Adds the effect, so do this last in this scenario.
	        }
	    }
    }
}
