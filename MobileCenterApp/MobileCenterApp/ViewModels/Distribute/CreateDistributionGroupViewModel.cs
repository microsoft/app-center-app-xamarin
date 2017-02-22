using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class CreateDistributionGroupViewModel : BaseViewModel
	{
		public CreateDistributionGroupViewModel()
		{
			Title = "Create Distribution Group";
			CreateCommand = new Command(async (obj) => await CreateDistirbutionGroup());
		}
		public ICommand CancelCommand { get; private set; } = new Command(async (obj) =>
		{
			await NavigationService.PopModalAsync();
		});

		public string Name { get; set; }


		public ICommand CreateCommand { get; private set; }

		public async Task CreateDistirbutionGroup()
		{
			IsLoading = true;
			try
			{
				if (string.IsNullOrWhiteSpace(Name))
				{
					await App.Current.MainPage.DisplayAlert("Error ", "Invalid Name", "Ok");
					return;
				}
				var app = Database.Main.GetObject<AppClass>(Settings.CurrentApp);
				var success = await SyncManager.Shared.CreateDistributionGroup(app, Name);
				if (success)
					await NavigationService.PopModalAsync();
				else
					await App.Current.MainPage.DisplayAlert("Error ", "There was an error creating the distribution group", "Ok");

			}
			catch (Exception ex)
			{
				LogManager.Shared.Report(ex);
				string message = "";
				if (ex.Data.Contains("HttpContent"))
				{
					message = ex.Data["HttpContent"].ToString();
				}
				else
					message = ex.Message;
				await App.Current.MainPage.DisplayAlert("Error", message, "Ok");
			}
			finally
			{
				IsLoading = false;
			}

		}
	}
}
