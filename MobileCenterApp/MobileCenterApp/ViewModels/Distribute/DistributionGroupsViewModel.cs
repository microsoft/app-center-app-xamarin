using System;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class DistributionGroupsViewModel : BaseViewModel
	{
		public DistributionGroupsViewModel()
		{
			Title = "Distribution";
			Icon = Images.DistributePageIcon;
		}

		SimpleDatabaseSource<DistributionGroup> items = new SimpleDatabaseSource<DistributionGroup>(Database.Main);
		public SimpleDatabaseSource<DistributionGroup> Items
		{
			get { return items; }
			set { ProcPropertyChanged(ref items, value); }
		}

		public AppClass CurrentApp { get; set; }

		public override async Task Refresh()
		{
			var currentId = Settings.CurrentApp;
			if (string.IsNullOrWhiteSpace(currentId))
				return;
			CurrentApp = Database.Main.GetObject<AppClass>(currentId);
			SetGroupInfo();
			await Task.Delay(1999);
			await SyncManager.Shared.SyncDistributionGroups(CurrentApp);
			SetGroupInfo();

		}
		void SetGroupInfo()
		{
			var groupInfo = Database.Main.GetGroupInfo<DistributionGroup>().Clone();
			groupInfo.OrderByDesc = true;
			groupInfo.Filter = $"AppId = ?";
			groupInfo.Params = CurrentApp?.Id;
			Items.GroupInfo = groupInfo;
		}
	}
}
