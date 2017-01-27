using System;
using System.Collections.Generic;

namespace MobileCenterApp
{
	public class AppListViewModel : BaseViewModel
	{
		public AppListViewModel()
		{
		}

		SimpleDatabaseSource<AppClass> items = new SimpleDatabaseSource<AppClass> { Database = Database.Main };
		public SimpleDatabaseSource<AppClass> Items { 
			get { return items; }
			set { ProcPropertyChanged(ref items, value); }
		} 
		async void Shared_AppsChanged(object sender, EventArgs e)
		{
			Items = new SimpleDatabaseSource<AppClass> { Database = Database.Main };
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();
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

		public override void OnDissapearing()
		{
			base.OnDissapearing();
			NotificationManager.Shared.AppsChanged -= Shared_AppsChanged;
		}

	}
}
