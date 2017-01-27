using System;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public class BasePage : ContentPage
	{
		public BasePage()
		{
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

