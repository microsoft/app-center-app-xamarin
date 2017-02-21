using System;
namespace MobileCenterApp
{
	public class NotificationManager
	{
		public static NotificationManager Shared { get; set; } = new NotificationManager();

		public event EventHandler AppsChanged;
		public void ProcAppsChanged()
		{
			AppsChanged?.InvokeOnMainThread(this);
		}

		public event EventHandler<EventArgs<string>> CurrentAppChanged;
		public void ProcAppsChanged(string appId)
		{
			CurrentAppChanged?.InvokeOnMainThread(this,appId);
		}

		public event EventHandler<EventArgs<bool>> OffineModeChanged;
		public void ProcOffineModeChanged(bool enabled)
		{
			OffineModeChanged?.InvokeOnMainThread(this, enabled);
		}

		public event EventHandler<EventArgs<string>> BranchesChanged;
		public void ProcBranchesChanged(string appId)
		{
			BranchesChanged?.InvokeOnMainThread(this, appId);
		}

		public event EventHandler<EventArgs<string>> BuildsChanged;
		public void ProcBuildsChanged(string appId)
		{
			BuildsChanged?.InvokeOnMainThread(this, appId);
		}

		public event EventHandler<EventArgs<string>> ReleaseDetailsChanged;
		public void ProcReleaseDetailsChanged(string releaseId)
		{
			ReleaseDetailsChanged?.InvokeOnMainThread(this, releaseId);
		}

		public event EventHandler<EventArgs<string>> DistributionGroupsChanged;
		public void ProcDistributionGroupsChanged(string appId)
		{
			DistributionGroupsChanged?.InvokeOnMainThread(this, appId);
		}

		public event EventHandler<EventArgs<string>> ReleasesChanged;
		public void ProcReleasesChanged(string appId)
		{
			ReleasesChanged?.InvokeOnMainThread(this, appId);
		}

		public event EventHandler<EventArgs<string>> DistributionGroupMembersChanged;
		public void ProcDistributionGroupMembersChanged(string distributionGroupId)
		{
			DistributionGroupMembersChanged?.InvokeOnMainThread(this, distributionGroupId);
		}

		public event EventHandler<EventArgs<string>> DistributionGroupReleasesChanged;
		public void ProcDistributionGroupReleasesChanged(string distributionGroupId)
		{
			DistributionGroupReleasesChanged?.InvokeOnMainThread(this, distributionGroupId);
		}
	}
}
