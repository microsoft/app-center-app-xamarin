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
			(BindingContext as BaseViewModel)?.OnAppearing();
			base.OnAppearing();
		}
		protected override void OnDisappearing()
		{
			(BindingContext as BaseViewModel)?.OnDisappearing();
			base.OnDisappearing();
		}
	}
}

