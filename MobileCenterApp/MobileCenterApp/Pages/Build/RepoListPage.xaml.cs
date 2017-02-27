using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class RepoListPage : BasePage
	{
		public RepoListPage()
		{
			InitializeComponent();
		}
		async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
			var item = e.SelectedItem as MobileCenterApi.Models.SourceRepository;
			if (item == null)
				return;
			await (BindingContext as RepoListViewModel)?.SelectRepo(item);
		}
	}
}
