using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class BranchDetailsPage : BasePage
	{
		public BranchDetailsPage()
		{
			InitializeComponent();
		}
		async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as Build;
			if (item == null)
				return;
			await (BindingContext as BranchDetailsViewModel)?.BuildSelected(item);
		}
	}
}
