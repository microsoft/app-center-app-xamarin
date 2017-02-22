using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Mobile;
using MobileCenterApi;
using SimpleAuth;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Web;
using System.Runtime.CompilerServices;

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
			SimpleIoC.RegisterPage<DistributeViewModel, DistributePage>();
			SimpleIoC.RegisterPage<CrashesViewModel, CrashesPage>();
			SimpleIoC.RegisterPage<AnalyticsViewModel, AnalyticsPage>();
			SimpleIoC.RegisterPage<BranchDetailsViewModel, BranchDetailsPage>();
			SimpleIoC.RegisterPage<RepoListViewModel, RepoListPage>();
			SimpleIoC.RegisterPage<BuildLogViewModel, BuildLogPage>();
			SimpleIoC.RegisterPage<ReleaseDetailsViewModel, ReleaseDetailsPage>();
			SimpleIoC.RegisterPage<DistributionGroupsViewModel, DistributionGroupsPage>();
			SimpleIoC.RegisterPage<CreateDistributionGroupViewModel, CreateDistributionGroupPage>();
			SimpleIoC.RegisterPage<DistributionGroupDetailsViewModel, DistributionGroupDetailsPage>();
			SimpleIoC.RegisterPage<InviteMemberViewModel, InviteMemberPage>();
		}

		public Action<string> OnInstallApp { get; set; }
		public async Task InstallRelease(Release release)
		{
			if (OnInstallApp == null)
				throw new NotImplementedException("OnInstallApp has not been implemented");

			//await MainPage.DisplayActionSheet("Do you want to install

			if (release.InstallUrl == null)
			{
				await SyncManager.Shared.SyncReleasesDetails(release);
			}
			OnInstallApp(release.InstallUrl);

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

		public static async void NotImplemented([CallerMemberName] string method = "",
							   [CallerFilePath] string sourceFilePath = "",
							   [CallerLineNumber] int sourceLineNumber = 0)
		{
			LogManager.Shared.LogEvent("Not Implemented", new Dictionary<string, string> {
				{ "Method", method },
				{"Line Number",sourceLineNumber.ToString()},
				{"FileName",sourceFilePath}
			});
			await Current.MainPage.DisplayAlert("Comming Soon", "This feature hasn't been implemented yet", "Ok");
		}
	}
}
