using System;
namespace MobileCenterApp
{
	public class RepoListViewModel : BaseViewModel
	{
		public RepoListViewModel()
		{
			Title = "Repositories";
		}
		public AppClass CurrentApp { get; set; }
	}
}
