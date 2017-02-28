using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class BranchDetailsViewModel : BaseViewModel
	{
		public ICommand QueueCommand { get; set; }
		public BranchDetailsViewModel()
		{
			Title = "Branch Details";
			QueueCommand = new Command(async (obj) => await QueueBranch());
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
			groupInfo.Filter = $"AppId = @AppId and SourceBranch = @SourceBranch";
			groupInfo.Params["@AppId"] = CurrentBranch?.AppId;
			groupInfo.Params["@SourceBranch"] = CurrentBranch?.Name;

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

		public async Task QueueBranch()
		{
			try
			{
				await SyncManager.Shared.QueueBranch(CurrentBranch);
				Items.ResfreshData();
			}
			catch (Exception ex)
			{
				LogManager.Shared.Report(ex);
				await App.Current.MainPage.DisplayAlert("Error", "There was an error Queing the build. Pleas try again.", "Ok");
			}
		}
	}
}
