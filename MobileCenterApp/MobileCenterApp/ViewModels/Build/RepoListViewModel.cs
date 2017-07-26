using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileCenterApi.Models;
using Xamarin.Forms;
using Exception = System.Exception;

namespace MobileCenterApp
{
	public class RepoListViewModel : BaseViewModel
	{
		public RepoListViewModel()
		{
			Title = "Repositories";
		}

		public ICommand CancelCommand { get; private set; } = new Command(async (obj) =>
		{
			await NavigationService.PopModalAsync();
		});

		public AppClass CurrentApp { get; set; }

		MobileCenterApi.Models.SourceRepository[] repositories;
		public MobileCenterApi.Models.SourceRepository[] Repositories
		{
			get { return repositories; }
			set { ProcPropertyChanged(ref repositories, value); }
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();
			await OnRefresh();
		}

		public override async Task OnRefresh()
		{
			Repositories = await SyncManager.Shared.Api.Build.List(SourceHost.Github, CurrentApp.Owner.Name, CurrentApp.Name, MobileCenterApi.Models.Form.Lite);
		}

		public async Task SelectRepo(MobileCenterApi.Models.SourceRepository repo)
		{
			IsLoading = true;
			try
			{
			    // TODO: The cast to string probably isn't correct here but at least makes it compile--make proper fix, also resolving the duplicate CloneUrl property issue
				var resp =await SyncManager.Shared.Api.Build.Update1(new MobileCenterApi.Models.RepoInfo { RepoUrl = (string) repo.CloneUrl }, CurrentApp.Owner.Name, CurrentApp.Name);
				Debug.WriteLine(resp.Message);
				await NavigationService.PopModalAsync();
			}
			catch (Exception ex)
			{
				if (ex.Data.Contains("HttpContent"))
				{
					Debug.WriteLine(ex.Data["HttpContent"]);
				}
				else
					Debug.WriteLine(ex);
				await App.Current.MainPage.DisplayAlert("Error", "There was an error adding the repo", "Ok");

			}
			finally
			{
				IsLoading = false;
			}
		}
	}
}
