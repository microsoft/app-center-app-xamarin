using System;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class DistributionGroupDetailsViewModel : BaseViewModel
	{
		public DistributionGroupDetailsViewModel()
		{
			
		}
		SimpleDatabaseSource<Tester> members = new SimpleDatabaseSource<Tester>(Database.Main);
		public SimpleDatabaseSource<Tester> Members
		{
			get { return members; }
			set { ProcPropertyChanged(ref members, value); }
		}

		SimpleDatabaseSource<DistributionReleaseGroup> releases = new SimpleDatabaseSource<DistributionReleaseGroup>(Database.Main) { IsGrouped = false };
		public SimpleDatabaseSource<DistributionReleaseGroup> Releases
		{
			get { return releases; }
			set { ProcPropertyChanged(ref releases, value); }
		}

		public string DistributionGroupId { get; set; }

		public DistributionGroup DistributionGroup
		{
			get { return Database.Main.GetObject<DistributionGroup>(DistributionGroupId); }
			set { DistributionGroupId = value?.Id; }
		}

		public override async Task OnRefresh()
		{
			var distributionGroup = DistributionGroup;
			await Task.WhenAll(SyncManager.Shared.SyncDistributionGroupMembers(distributionGroup), SyncManager.Shared.SyncDistributionGroupReleases(distributionGroup));
		}
		public override void OnAppearing()
		{
			SetMemberGroupInfo();
			SetReleaseGroupInfo();
			NotificationManager.Shared.DistributionGroupMembersChanged += Shared_DistributionGroupMembersChanged;
			NotificationManager.Shared.DistributionGroupReleasesChanged += Shared_DistributionGroupReleasesChanged;
			base.OnAppearing();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			NotificationManager.Shared.DistributionGroupMembersChanged -= Shared_DistributionGroupMembersChanged;
			NotificationManager.Shared.DistributionGroupReleasesChanged -= Shared_DistributionGroupReleasesChanged;
		}

		void SetMemberGroupInfo()
		{
			var distributionGroup = DistributionGroup;
			var groupInfo = Database.Main.GetGroupInfo<Tester>().Clone();
			groupInfo.Filter = "DistributionId = ?";
			groupInfo.Params = distributionGroup?.Id;
			Members.GroupInfo = groupInfo;
		}

		void SetReleaseGroupInfo()
		{
			var distributionGroup = DistributionGroup;
			var groupInfo = Database.Main.GetGroupInfo<DistributionReleaseGroup>().Clone();
			groupInfo.GroupOrderByDesc = true;
			groupInfo.OrderByDesc = true;
			groupInfo.Filter = "DistributionId = ?";
			groupInfo.Params = DistributionGroup?.Id;
			Releases.GroupInfo = groupInfo;
		}

		void Shared_DistributionGroupMembersChanged(object sender, MobileCenterApp.EventArgs<string> e)
		{
			SetMemberGroupInfo();
		}

		void Shared_DistributionGroupReleasesChanged(object sender, MobileCenterApp.EventArgs<string> e)
		{
			SetReleaseGroupInfo();
		}
	}
}
