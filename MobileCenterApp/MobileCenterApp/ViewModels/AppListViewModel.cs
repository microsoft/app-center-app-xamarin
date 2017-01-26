using System;
using System.Collections.Generic;

namespace MobileCenterApp
{
	public class AppListViewModel : BaseViewModel
	{
		public AppListViewModel()
		{
		}

		public IListDatabaseSource<AppClass> Items { get; set; } = new IListDatabaseSource<AppClass> { Database = Database.Main };
		//public List<AppClass> Items { get; set; }
		async void Shared_AppsChanged(object sender, EventArgs e)
		{
			//Items = await Database.Main.TablesAsync<AppClass>().ToListAsync();
			ProcPropertyChanged("Items");
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();
			//Items = await Database.Main.TablesAsync<AppClass>().ToListAsync();
			ProcPropertyChanged("Items");
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
