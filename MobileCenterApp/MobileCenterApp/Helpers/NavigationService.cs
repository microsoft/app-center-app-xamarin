using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public static class NavigationService
	{
		public static async Task PushAsync (BaseViewModel viewModel)
		{
			var view = SimpleIoC.GetPage (viewModel.GetType ());
			if (view == null)
			{
				throw new NotImplementedException($"There is no Page registered with {viewModel.GetType()}. Please register the page and view model with SimpleIoC");
			}
			view.BindingContext = viewModel;
			await Navigation.PushAsync (view);

		}

		public static async Task PopAsync ()
		{
			await Navigation.PopAsync ();
		}

		public static async Task PushModalAsync (BaseViewModel viewModel,bool wrapInNavigation = true)
		{
			var view = SimpleIoC.GetPage (viewModel.GetType ());
			view.BindingContext = viewModel;
			await Navigation.PushModalAsync (wrapInNavigation ? new NavigationPage(view) : view);
		}

		public static async Task PopModalAsync ()
		{
			await Navigation.PopModalAsync ();
		}

		public static void SetRoot (object viewModel,bool wrapInNavigation = true)
		{
			var view = SimpleIoC.GetPage (viewModel.GetType ()); 
			view.BindingContext = viewModel;
			Application.Current.MainPage = wrapInNavigation ? new NavigationPage(view) : view;
		}

		static INavigation Navigation
		{
			get
			{
				var masterDetail = Application.Current.MainPage as MasterDetailPage;
				var currentPage = masterDetail?.Detail ?? Application.Current.MainPage;
				//If the tab page has Navigation controllers as the contents, we need to use those.
				var tabbed = currentPage as TabbedPage;
				return tabbed?.CurrentPage?.Navigation ?? currentPage.Navigation;
			}
		}
	}
}

