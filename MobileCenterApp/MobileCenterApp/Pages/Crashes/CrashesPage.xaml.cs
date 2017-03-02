using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class CrashesPage : BasePage
	{
		public CrashesPage()
		{
			InitializeComponent();
		}

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var crashGroup = e.SelectedItem as CrashGroup;
			if (crashGroup == null)
				return;
			(BindingContext as CrashesViewModel).CrashSelected(crashGroup);
		}
	}
}
