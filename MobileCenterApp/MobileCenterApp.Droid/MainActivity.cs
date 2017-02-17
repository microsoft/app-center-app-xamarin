using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace MobileCenterApp.Droid
{
	[Activity (Label = "MobileCenterApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		MobileCenterApp.App App;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

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

