using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
			await Refresh();
		}

		public override async Task Refresh()
		{
			Repositories = await SyncManager.Shared.Api.Build.GetRepositories("github", CurrentApp.Owner.Name, CurrentApp.Name, MobileCenterApi.Models.Form.Lite);
		}

		public async Task SelectRepo(MobileCenterApi.Models.SourceRepository repo)
		{
			IsLoading = true;
			try
			{
				var resp =await SyncManager.Shared.Api.Build.CreateRepositoryConfiguration(new MobileCenterApi.Models.RepoInfo { RepoUrl = repo.CloneUrl }, CurrentApp.Owner.Name, CurrentApp.Name);
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
