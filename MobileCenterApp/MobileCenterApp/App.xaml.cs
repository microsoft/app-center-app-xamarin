using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Mobile;
using MobileCenterApi;
using SimpleAuth;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public partial class App : Application
	{
		public App()
		{
			RegisterViewModels();
			BasicAuthApi.ShowAuthenticator = (IBasicAuthenicator obj) =>
			{
				Xamarin.Forms.Device.BeginInvokeOnMainThread(()=>MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage(obj))));
			};
			InitializeComponent();
			// The root page of your application
			MainPage = SimpleIoC.GetPage(new MainPageViewModel());
		}

		void RegisterViewModels()
		{
			SimpleIoC.RegisterPage<AppListViewModel, AppListPage>();
			SimpleIoC.RegisterPage<CreateAppViewModel, CreateAppPage>();
			SimpleIoC.RegisterPage<MainPageViewModel, MainPage>();
			SimpleIoC.RegisterPage<GettingStartedViewModel, GettingStartedPage>();
			SimpleIoC.RegisterPage<BuildViewModel, BuildPage>();
			SimpleIoC.RegisterPage<TestViewModel,TestPage>();
		}


		protected override void OnStart()
		{

		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
