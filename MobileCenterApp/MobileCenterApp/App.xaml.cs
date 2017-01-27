using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Mobile;
using MobileCenterApi;
using SimpleAuth;
using Xamarin.Forms;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class App : Application
	{
		public App()
		{
			RegisterViewModels();
			BasicAuthApi.ShowAuthenticator = (IBasicAuthenicator obj) =>
			{
				MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage(obj)));
			};
			InitializeComponent();
			// The root page of your application
			NotificationManager.Shared.CurrentAppChanged += Shared_CurrentAppChanged;
			SetRoot();
		}

		void SetRoot()
		{
			var model = string.IsNullOrWhiteSpace(Settings.CurrentApp) ? (BaseViewModel)new AppListViewModel() : new MainPageViewModel();
			NavigationService.SetRoot(model);
		}

		void RegisterViewModels()
		{
			SimpleIoC.RegisterPage<AppListViewModel, AppListPage>();
			SimpleIoC.RegisterPage<MainPageViewModel, MainPage>();
		}

		void Shared_CurrentAppChanged(object sender, MobileCenterApp.EventArgs<string> e)
		{
			SetRoot();
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
