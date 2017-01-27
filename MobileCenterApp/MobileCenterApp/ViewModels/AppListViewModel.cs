using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class AppListViewModel : BaseViewModel
	{
		public AppListViewModel()
		{
			Title = "Apps";
		}

		SimpleDatabaseSource<AppClass> items;
		public SimpleDatabaseSource<AppClass> Items { 
			get { return items; }
			set { ProcPropertyChanged(ref items, value); }
		}

		public ICommand CreateCommand { get; private set; } = new Command(async (obj) =>
		{
			try
			{
				await NavigationService.PushModalAsync(new CreateAppViewModel());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		});

		void Shared_AppsChanged(object sender, EventArgs e)
		{
			Items = new SimpleDatabaseSource<AppClass> { Database = Database.Main };
		}


		public override async void OnAppearing()
		{
			base.OnAppearing();
			Items = new SimpleDatabaseSource<AppClass> { Database = Database.Main };
			NotificationManager.Shared.AppsChanged += Shared_AppsChanged;
			try
			{
				await SyncManager.Shared.SyncApps();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			NotificationManager.Shared.AppsChanged -= Shared_AppsChanged;
			this.ClearEvents();
		}
	}
}
