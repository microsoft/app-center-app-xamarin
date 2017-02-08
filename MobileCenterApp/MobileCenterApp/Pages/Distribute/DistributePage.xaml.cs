using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class DistributePage : BasePage
	{
		public DistributePage()
		{
			InitializeComponent();
		}

		async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as Release;
			if (item == null)
				return;
		}
	}
}
