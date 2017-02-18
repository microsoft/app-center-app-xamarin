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

		SimpleDatabaseSource<Build> items = new SimpleDatabaseSource<Build>(Database.Main) { IsGrouped = false};
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
				{
					SetGroupInfo();
					Title = CurrentBranch?.Name ?? "Branch Details";
				}
			}
		}

		public override async Task OnRefresh()
		{
			Items.ResfreshData();
			await SyncManager.Shared.SyncBuilds(CurrentBranch);
			SetGroupInfo();
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
		protected override void LoggingPageView()
		{
			LogManager.Shared.PageView("Branch Details");
		}
	}
}
