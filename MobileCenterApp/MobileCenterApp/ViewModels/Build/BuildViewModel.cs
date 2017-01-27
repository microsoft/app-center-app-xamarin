using System;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class BuildViewModel : BaseViewModel
	{
		public BuildViewModel()
		{
			Title = "Build";
			Icon = Images.BuildPageIcon;
		}

		SimpleDatabaseSource<Branch> items;
		public SimpleDatabaseSource<Branch> Items
		{
			get { return items; }
			set { ProcPropertyChanged(ref items, value); }
		}

		void Shared_AppsChanged(object sender, EventArgs e)
		{
			SetupItems();
		}

		void SetupItems()
		{
			var groupInfo = Database.Main.GetGroupInfo<Branch>().Clone();
			groupInfo.Filter = "AppId = ?";
			groupInfo.Params = CurrentApp?.Id;
			Items = new SimpleDatabaseSource<Branch> { Database = Database.Main, GroupInfo = groupInfo };
		}

		public AppClass CurrentApp { get; set; }

		public override async void OnAppearing()
		{
			base.OnAppearing();
			SetCurrentApp();
			SetupItems();
			NotificationManager.Shared.AppsChanged += Shared_AppsChanged;
			NotificationManager.Shared.BranchesChanged += Shared_BranchesChanged;
			try
			{
				await SyncManager.Shared.SyncBranch(CurrentApp);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			NotificationManager.Shared.AppsChanged -= Shared_AppsChanged;
			NotificationManager.Shared.BranchesChanged -= Shared_BranchesChanged;
		}

		async void SetCurrentApp()
		{
			var currentId = Settings.CurrentApp;
			if (string.IsNullOrWhiteSpace(currentId))
				await NavigationService.PushModalAsync(new AppListViewModel());
			if (String.IsNullOrWhiteSpace(currentId) || CurrentApp?.Id == currentId)
				return;
			CurrentApp = Database.Main.GetObject<AppClass>(currentId);
		}


		void Shared_BranchesChanged(object sender, MobileCenterApp.EventArgs<string> e)
		{
			if (e.Data != CurrentApp?.Id)
				return;
			SetupItems();
		}

		public Task BranchSelected(Branch branch)
		{
			return NavigationService.PushAsync(new BranchDetailsViewModel { CurrentBranch = branch });
		}
	}
}
