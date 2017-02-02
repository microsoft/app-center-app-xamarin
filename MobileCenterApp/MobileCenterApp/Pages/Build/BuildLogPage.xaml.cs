using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class BuildLogPage : BasePage
	{
		public BuildLogPage()
		{
			InitializeComponent();
			RazorView.RazorTemplate = new LogRazorTemplate();
		}
	}
}
