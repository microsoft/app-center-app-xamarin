using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class ReleaseDetailsViewModel : BaseViewModel
	{
        public ICommand InstallAppCommand { protected set; get; }
		public ReleaseDetailsViewModel()
		{
			InstallAppCommand = new Command(async (obj) => await InstallApp());
		}

		Release release;
		public Release Release
		{
			get { return release; }
			set {
				if (this.ProcPropertyChanged(ref release, value))
				{
					Title = $"{value?.AppName} - {value?.Version}({value?.ShortVersion})";
				}
			}
		}
		public override async Task Refresh()
		{
			if (Release == null)
				return;
			await SyncManager.Shared.SyncReleasesDetails(Release);
			//Refresh and get a new one from the DB
			Release = Database.Main.GetObject<Release>(Release.ReleaseId);
		}

		public async Task InstallApp()
		{
			await (App.Current as App).InstallRelease(Release);
		}
	}
}
