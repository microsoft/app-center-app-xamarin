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

		public override void OnAppearing()
		{
			base.OnAppearing();
			NotificationManager.Shared.AppsChanged += Shared_AppsChanged;
			NotificationManager.Shared.BranchesChanged += Shared_BranchesChanged;
			SetupData();
		}

		async void SetupData()
		{
			SetCurrentApp();
			IsLoading = true;
			var syncRepoTask = SyncManager.Shared.SyncRepoConfig(CurrentApp);
			//Lets check if there are any repo configs for this app;
			var hasRepoConfigs = HasRepoConfigs();
			if (hasRepoConfigs)
				await SetupBranches();

			try
			{
				await syncRepoTask;
			}
			catch (Exception ex)
			{
				if (ex.Data.Contains("HttpContent"))
				{
					Console.WriteLine(ex.Data["HttpContent"]);
				}
				else
					Console.WriteLine(ex);
			}
			//Lets check again after the sync!
			if (hasRepoConfigs || HasRepoConfigs())
			{
				await SetupBranches();
			}
			else {
				var shouldAddRepo = await App.Current.MainPage.DisplayAlert("No Repo", "Would you like to associate a repo now?", "Ok", "Maybe later");
				if (shouldAddRepo)
					await NavigationService.PushModalAsync(new RepoListViewModel { CurrentApp = CurrentApp});
			}

			IsLoading = false;
		}

		bool HasRepoConfigs()
		{
			var groupInfo = Database.Main.GetGroupInfo<RepoConfig>().Clone();
			groupInfo.Filter = "AppId = ?";
			groupInfo.Params = CurrentApp?.Id;
			var count = Database.Main.RowsInSection<RepoConfig>(groupInfo,0);
			return count > 0;
		}

		async Task SetupBranches()
		{
			SetupItems();
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
