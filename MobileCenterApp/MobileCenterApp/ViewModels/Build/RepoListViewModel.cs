using System;
using System.Collections.Generic;
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

		MobileCenterApi.SourceRepository[] repositories;
		public MobileCenterApi.SourceRepository[] Repositories
		{
			get { return repositories; }
			set { ProcPropertyChanged(ref repositories, value); }
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();
			await Refresh();
		}

		public async Task Refresh()
		{
			IsLoading = true;
			try
			{
				Repositories = await SyncManager.Shared.Api.BuildGetRepositories("github", CurrentApp
																  .Owner.Name, CurrentApp.Name, "lite");
			}
			catch (Exception ex)
			{
				if (ex.Data.Contains("HttpContent"))
				{
					Console.WriteLine(ex.Data["HttpContent"]);
				}
				else
					Console.WriteLine(ex);
			}
			finally
			{
				IsLoading = false;
			}
		}

		public async Task SelectRepo(MobileCenterApi.SourceRepository repo)
		{
			IsLoading = true;
			try
			{
				var resp =await SyncManager.Shared.Api.PostBuildCreateRepositoryConfiguration(new MobileCenterApi.RepoInfo { RepoUrl = repo.CloneUrl }, CurrentApp.Owner.Name, CurrentApp.Name);
				Console.WriteLine(resp.Message);
				await NavigationService.PopModalAsync();
			}
			catch (Exception ex)
			{
				if (ex.Data.Contains("HttpContent"))
				{
					Console.WriteLine(ex.Data["HttpContent"]);
				}
				else
					Console.WriteLine(ex);
				await App.Current.MainPage.DisplayAlert("Error", "There was an error adding the repo", "Ok");

			}
			finally
			{
				IsLoading = false;
			}
		}
	}
}
