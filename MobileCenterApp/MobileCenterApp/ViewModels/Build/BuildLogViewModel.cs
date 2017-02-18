using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class BuildLogViewModel : BaseViewModel
	{
		public BuildLogViewModel()
		{
			Title = "Build Log";
		}

		List<LogSection> logs;
		public List<LogSection> Logs
		{
			get { return logs; }
			set { ProcPropertyChanged(ref logs, value); }
		}

		Build currentBuild;
		public Build CurrentBuild { 
			get { return currentBuild;}
			set {
				if (ProcPropertyChanged(ref currentBuild, value))
				{
					Title = $"{currentBuild.BuildId} : Logs";
				}
			}
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();
		}

		public override async Task OnRefresh()
		{
			if (logs?.Count > 0 || CurrentBuild == null)
				return;

			//var app = Database.Main.GetObject<AppClass>(CurrentBuild.AppId);
			//var l = await SyncManager.Shared.Api.BuildGetBuildLogs(CurrentBuild.BuildId, app.Owner.Name, app.Name);
			//Logs = l.Logs;
			var data = await SyncManager.Shared.DownloadLog(CurrentBuild);
			Logs = data;
		}
		protected override void LoggingPageView()
		{
			LogManager.Shared.PageView("Build Logs");
		}
	}
}
