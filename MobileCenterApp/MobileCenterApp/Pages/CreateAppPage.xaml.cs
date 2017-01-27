using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class CreateAppPage : ContentPage
	{
		public CreateAppPage()
		{
			InitializeComponent();
			//TODO: Add Enum support for OS/Platform
		}
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			//TODO: Remove this when Forms supports Picker binding;
			var model = BindingContext as CreateAppViewModel;
			OsPicker.Items.Clear();
			foreach (var os in model.OsOptions)
			{
				OsPicker.Items.Add(os);
			}

			PlatformPicker.Items.Clear();
			foreach (var os in model.Platforms)
			{
				PlatformPicker.Items.Add(os);
			}
		}
	}
}
