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
	}
}
