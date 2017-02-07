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

		public override async Task Refresh()
		{
			var app = Database.Main.GetObject<AppClass>(Settings.CurrentApp);
			var crashes = await SyncManager.Shared.Api.GetCrashGroups(app.Owner.Name, app.Name);
			Debug.WriteLine(crashes);
		}
	}
}
