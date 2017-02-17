using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace MobileCenterApp.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		App formsApp;
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			MobileCenter.Start("894ddd86-3771-44db-bf3a-fca3385abf68",
					typeof(Analytics), typeof(Crashes));
			global::Xamarin.Forms.Forms.Init ();
			LoadApplication (formsApp = new MobileCenterApp.App ());
			//Install apps
			formsApp.OnInstallApp = (installUrl) => UIApplication.SharedApplication.OpenUrl(new NSUrl($"itms-services://?action=download-manifest&url={installUrl}"));

			return base.FinishedLaunching (app, options);
		}
	}
}
