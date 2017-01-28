using System;
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

		public virtual void OnAppearing()
		{

		}

		public virtual void OnDisappearing()
		{

		}
	}
}
