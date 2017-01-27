using System;
using System.Linq;

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

		public override void OnAppearing()
		{
			base.OnAppearing();
			SetCurrentApp();
			SetupCurrentUser();
			NotificationManager.Shared.CurrentAppChanged += Shared_CurrentAppChanged;
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			NotificationManager.Shared.CurrentAppChanged -= Shared_CurrentAppChanged;
		}

		void SetCurrentApp()
		{
			var currentId = Settings.CurrentApp;
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
				Console.WriteLine(ex);
			}
		}

		void Shared_CurrentAppChanged(object sender, MobileCenterApp.EventArgs<string> e)
		{
			SetCurrentApp();
		}
	}
}
