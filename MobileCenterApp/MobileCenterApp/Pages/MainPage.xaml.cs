using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			(BindingContext as BaseViewModel)?.OnAppearing();
			SetContent();
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			(BindingContext as BaseViewModel)?.OnDisappearing();
		}

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var model = BindingContext as MainPageViewModel;
			var viewModel = e.SelectedItem as BaseViewModel;
			model.CurrentPage = viewModel;
			SetContent();
			IsPresented = false;
		}

		void SetContent()
		{
			var viewModel = (BindingContext as MainPageViewModel).CurrentPage;
			var page = SimpleIoC.GetPage(viewModel);
			this.Detail = new NavigationPage(page);
		}
	}
}
