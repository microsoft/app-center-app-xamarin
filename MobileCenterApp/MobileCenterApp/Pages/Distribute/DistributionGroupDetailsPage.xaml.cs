using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class DistributionGroupDetailsPage : TabbedPage
	{
		public DistributionGroupDetailsPage()
		{
			InitializeComponent();
			this.SetBinding(TitleProperty, "Title");
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			(BindingContext as BaseViewModel)?.OnAppearing();
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			(BindingContext as BaseViewModel)?.OnDisappearing();
		}
	}
}
