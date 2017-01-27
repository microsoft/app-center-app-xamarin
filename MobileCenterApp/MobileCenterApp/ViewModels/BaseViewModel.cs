using System;
namespace MobileCenterApp
{
	public class BaseViewModel : BaseModel
	{
		public string Title { get; set; }

		public string Icon { get; set; }

		public virtual void OnAppearing()
		{

		}

		public virtual void OnDisappearing()
		{

		}
	}
}
