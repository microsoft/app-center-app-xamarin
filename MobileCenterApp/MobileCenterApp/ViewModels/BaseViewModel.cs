using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class BaseViewModel : BaseModel
	{
		public BaseViewModel()
		{
			RefreshCommand = new Command(async (obj) => await Refresh());
		}
		string title;
		public virtual string Title
		{
			get { return title; }
			set { ProcPropertyChanged(ref title, value); }
		}

		public string Icon { get; set; }

		bool isLoading;
		public bool IsLoading
		{
			get { return isLoading; }
			set { ProcPropertyChanged(ref isLoading, value); }
		}

		public virtual async void OnAppearing()
		{
			LoggingPageView();
			await Refresh();

		}

		protected virtual void LoggingPageView()
		{
			LogManager.Shared.PageView(Title);
		}
		public virtual void OnDisappearing()
		{

		}

		public ICommand RefreshCommand { get; set; }

		Task refreshTask;
		public Task Refresh()
		{
			if (refreshTask?.IsCompleted ?? true)
				refreshTask = refresh();
			return refreshTask;
			
		}
		async Task refresh()
		{
			var r = OnRefresh();
			if (r.IsCompleted)
				return;
			IsLoading = true;
			try
			{
				await r;
			}
			catch (Exception ex)
			{
				if (ex.Data.Contains("HttpContent"))
				{
					Debug.WriteLine(ex.Data["HttpContent"]);
				}
				else
					Debug.WriteLine(ex);
			}
			finally
			{
				IsLoading = false;
			}
		}

		public virtual Task OnRefresh()
		{
			return Task.FromResult(true);
		}
	}
}
