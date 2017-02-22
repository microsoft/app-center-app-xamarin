using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace MobileCenterApp.Droid
{
	[Activity (Label = "MobileCenterApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnStart()
		{
			base.OnStart();
			MobileCenter.Start(typeof(Analytics), typeof(Crashes));
		}
		MobileCenterApp.App App;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			MobileCenter.Configure("a635d99d-b70e-42ca-aa1e-61484e2fa40d");
			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (App = new MobileCenterApp.App ());
			App.OnInstallApp = (installUrl) =>
			{
				var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(installUrl));
				intent.SetFlags(ActivityFlags.NewTask);
				Android.App.Application.Context.StartActivity(intent);
			};
		}
	}
}

