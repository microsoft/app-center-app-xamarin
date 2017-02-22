using System;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public class DistributionTesterDetails : ContentPage
	{
		public DistributionTesterDetails()
		{
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}

