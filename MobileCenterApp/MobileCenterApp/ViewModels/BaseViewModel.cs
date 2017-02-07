using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public class BaseViewModel : BaseModel
	{
		string title;
		public string Title
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
			var refresh = Refresh();
			if (refresh.IsCompleted)
				return;
			IsLoading = true;
			try
			{
				await refresh;
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

		protected virtual void LoggingPageView()
		{
			LogManager.Shared.PageView(Title);
		}
		public virtual void OnDisappearing()
		{

		}

		public virtual Task Refresh()
		{
			return Task.FromResult(true);
		}
	}
}
