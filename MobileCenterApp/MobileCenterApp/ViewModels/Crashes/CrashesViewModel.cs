using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class CrashesViewModel : BaseViewModel
	{
		public CrashesViewModel()
		{
			Title = "Crashes";
			Icon = Images.CrashesPageIcon;
		}

		SimpleDatabaseSource<CrashGroup> items = new SimpleDatabaseSource<CrashGroup>(Database.Main) { IsGrouped = false };
		public SimpleDatabaseSource<CrashGroup> Items
		{
			get { return items; }
			set { ProcPropertyChanged(ref items, value); }
		}

		public override async Task OnRefresh()
		{
			SetGroupInfo();
			await SyncManager.Shared.SyncCrashGroups(Settings.CurrentApp);
			SetGroupInfo();
		}

		CrashGroupStatus statusFilter = CrashGroupStatus.Open;
		public CrashGroupStatus StatusFilter
		{
			get { return statusFilter; }
			set { if (ProcPropertyChanged(ref statusFilter, value)) SetGroupInfo(); }
		}

		void SetGroupInfo()
		{
			var groupInfo = Database.Main.GetGroupInfo<CrashGroup>().Clone();
			groupInfo.GroupOrderByDesc = true;
			groupInfo.OrderByDesc = true;
			groupInfo.Filter = $"AppId = @AppId and Status = @Status";
			groupInfo.Params["@AppId"] = Settings.CurrentApp?.Id;
			groupInfo.Params["@Status"] = CrashGroupStatus.Open;
			Items.GroupInfo = groupInfo;
		}

		public async void CrashSelected(CrashGroup crashGroup)
		{
			await NavigationService.PushAsync(new CrashDetailsViewModel { CrashGroupId = crashGroup.Id });
		}

	}
}
