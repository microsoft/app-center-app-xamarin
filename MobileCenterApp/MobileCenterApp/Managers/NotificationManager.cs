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
	}
}
