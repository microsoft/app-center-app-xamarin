using System;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class CrashDetailsViewModel : BaseViewModel
	{
		public CrashDetailsViewModel()
		{
			Title = "Crash Details";
		}

		StackTrace stackTrace;
		public StackTrace StackTrace
		{
			get { return stackTrace; }
			set { ProcPropertyChanged(ref stackTrace, value); }
		}

		public string CrashGroupId { get; set; }

		CrashGroup CrashGroup => Database.Main.GetObject<CrashGroup>(CrashGroupId);

		public override async Task OnRefresh()
		{
			StackTrace = Database.Main.GetObject<StackTrace>(CrashGroupId);
			await SyncManager.Shared.SyncStackTrace(CrashGroup);
			StackTrace = Database.Main.GetObject<StackTrace>(CrashGroupId);
		}

	}
}
