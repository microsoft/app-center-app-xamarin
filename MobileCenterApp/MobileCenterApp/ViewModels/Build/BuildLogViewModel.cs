using System;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class BuildLogViewModel : BaseViewModel
	{
		public BuildLogViewModel()
		{
			Title = "Logs";
		}

		string[] logs;
		public string[] Logs
		{
			get { return logs; }
			set { ProcPropertyChanged(ref logs, value); }
		}

		public Build CurrentBuild { get; set; }

		public override async void OnAppearing()
		{
			base.OnAppearing();
		}

		public override async Task Refresh()
		{
			if (logs?.Length > 0 || CurrentBuild == null)
				return;
			
			var app = Database.Main.GetObject<AppClass>(CurrentBuild.AppId);
			var l = await SyncManager.Shared.Api.BuildGetBuildLogs(CurrentBuild.BuildId, app.Owner.Name, app.Name);
			Logs = l.Logs;
		}
	}
}
