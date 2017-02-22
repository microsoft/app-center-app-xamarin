using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class DistributionGroupDetailsViewModel : BaseViewModel
	{
		public ICommand AddMemberCommand { get; private set; } 
		public DistributionGroupDetailsViewModel()
		{
			AddMemberCommand = new Command(async (obj) => {
				await NavigationService.PushModalAsync(new InviteMemberViewModel { DistributionGroup = DistributionGroup });
			});
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

		string distributionId;
		public string DistributionGroupId { 
			get { return distributionId; }
			set
			{
				if (ProcPropertyChanged(ref distributionId, value))
				{
					SetMemberGroupInfo();
					SetReleaseGroupInfo();
				}

			}
		}

		public DistributionGroup DistributionGroup
		{
			get { return Database.Main.GetObject<DistributionGroup>(DistributionGroupId); }
			set {
				DistributionGroupId = value?.Id;
			}
		}

		public override async Task OnRefresh()
		{
			var distributionGroup = DistributionGroup;
			await Task.WhenAll(SyncManager.Shared.SyncDistributionGroupMembers(distributionGroup), SyncManager.Shared.SyncDistributionGroupReleases(distributionGroup));
		}
		bool isHookedUp;
		public override void OnAppearing()
		{
			base.OnAppearing();
			if (isHookedUp)
				return;
			isHookedUp = true;
			NotificationManager.Shared.DistributionGroupMembersChanged += Shared_DistributionGroupMembersChanged;
			NotificationManager.Shared.DistributionGroupReleasesChanged += Shared_DistributionGroupReleasesChanged;

		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			if (!isHookedUp)
				return;
			isHookedUp = false;
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
