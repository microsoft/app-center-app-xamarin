using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class MainPageViewModel : BaseViewModel
	{
		public MainPageViewModel()
		{
			Title = "Mobile Center";
		}

		public BaseViewModel[] Children { get; set; } = new BaseViewModel[]{
			new GettingStartedViewModel(),
			new BuildViewModel(),
			new TestViewModel(),
			new DistributeViewModel(),
			new CrashesViewModel(),
			new AnalyticsViewModel(),
		};

		AppClass currentApp;
		public AppClass CurrentApp {
			get { return currentApp ; }
			set { ProcPropertyChanged(ref currentApp, value); }
		}

		User currentUser;
		public User CurrentUser
		{
			get { return currentUser; }
			set { ProcPropertyChanged(ref currentUser, value); }
		}

		BaseViewModel currentPage;
		public BaseViewModel CurrentPage { 
			get { return currentPage ?? Children.FirstOrDefault(); }
			set { ProcPropertyChanged(ref currentPage, value); }
		}

		public ICommand SwitchAppsCommand { get; private set; } = new Command(async (obj) =>
		{
			try
			{
				await NavigationService.PushModalAsync(new AppListViewModel());
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		});

		public override void OnAppearing()
		{
			base.OnAppearing();
			NotificationManager.Shared.CurrentAppChanged += Shared_CurrentAppChanged;
			SetCurrentApp();
			SetupCurrentUser();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			NotificationManager.Shared.CurrentAppChanged -= Shared_CurrentAppChanged;
		}

		async void SetCurrentApp()
		{
			var currentId = Settings.CurrentApp;
			CurrentApp = Database.Main.GetObject<AppClass>(currentId);
			if (string.IsNullOrWhiteSpace(currentId) || CurrentApp == null)
				await NavigationService.PushModalAsync(new AppListViewModel());
			if (String.IsNullOrWhiteSpace(currentId) || CurrentApp?.Id == currentId)
				return;
			CurrentApp = Database.Main.GetObject<AppClass>(currentId);
		}

		async void SetupCurrentUser()
		{
			try
			{
				var user = Settings.CurrentUser;
				if (user == null)
					user = await SyncManager.Shared.GetUser();
				CurrentUser = user;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		void Shared_CurrentAppChanged(object sender, MobileCenterApp.EventArgs<string> e)
		{
			SetCurrentApp();
		}
	}
}
