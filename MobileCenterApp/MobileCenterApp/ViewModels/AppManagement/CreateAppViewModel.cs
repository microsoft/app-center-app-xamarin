using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class CreateAppViewModel : BaseViewModel
	{
		public CreateAppViewModel()
		{
			Title = "Add new app";
			CreateCommand = new Command(async (obj) => await AddApp());
			PlatformIndex = 0;
			OsIndex = 0;
		}
		public AppClass App { get; set; } = new AppClass();

		//TODO: get this from the API
		public string[] OsOptions = new string[]{
			"iOS",
			"Android",
		};
		public int OsIndex
		{
			get { return Array.IndexOf(OsOptions, App.Os); }
			set {
				if (value < 0 || value >= OsOptions.Length)
					App.Os = "";
				else
					App.Os = OsOptions[value];
			}
		}

		//TODO: get this from the API
		public string[] Platforms = new string[]{
			"Objective-C-Swift",
			"Java",
			"React-Native",
			"Xamarin"
		};
		public int PlatformIndex
		{
			get { return Array.IndexOf(Platforms, App.Platform); }
			set {

				if (value < 0 || value >= Platforms.Length)
					App.Platform = "";
				else
					App.Platform = Platforms[value]; }
		}



		public ICommand CreateCommand { get; private set; }
		async Task AddApp()
		{

			//TODO: validate the app.
			App.DisplayName = App.Name;
			IsLoading = true;
			var success = await SyncManager.Shared.CreateApp(this.App);
			IsLoading = false;
			if (success)
				await NavigationService.PopModalAsync();
			//TODO: Show error
			await MobileCenterApp.App.Current.MainPage.DisplayActionSheet($"Error: Invalid data", "Ok",null);
		}

		public ICommand CancelCommand { get; private set; } = new Command(async (obj) =>
		{
			await NavigationService.PopModalAsync();
		});

	}
}
