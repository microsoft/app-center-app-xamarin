using System;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class BranchDetailsViewModel : BaseViewModel
	{
		public BranchDetailsViewModel()
		{
			Title = "Branch Details";
		}

		SimpleDatabaseSource<Build> items = new SimpleDatabaseSource<Build> { Database = Database.Main, GroupInfo = new SimpleDatabase.GroupInfo { } };
		public SimpleDatabaseSource<Build> Items
		{
			get { return items; }
			set { ProcPropertyChanged(ref items, value); }
		}

		Branch currentBranch;
		public Branch CurrentBranch
		{
			get { return currentBranch; }
			set
			{
				if (ProcPropertyChanged(ref currentBranch, value))
					SetGroupInfo();
			}
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();
			await RefreshData();
		}

		public async Task RefreshData()
		{
			Items?.ResfreshData();
			IsLoading = true;
			try
			{
				await SyncManager.Shared.SyncBuilds(CurrentBranch);
				SetGroupInfo();
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
			finally
			{
				IsLoading = false;
			}
			Items?.ResfreshData();

		}

		void SetGroupInfo()
		{
			var groupInfo = Database.Main.GetGroupInfo<Build>().Clone();
			groupInfo.GroupOrderByDesc = true;
			groupInfo.OrderByDesc = true;
			groupInfo.Filter = $"AppId = '{CurrentBranch?.AppId}' and SourceBranch = ?";
			groupInfo.Params = CurrentBranch?.Name;
			Items.GroupInfo = groupInfo;
		}

		public async Task BuildSelected(Build build)
		{
			await NavigationService.PushAsync(new BuildLogViewModel { CurrentBuild = build });
		}
	}
}
