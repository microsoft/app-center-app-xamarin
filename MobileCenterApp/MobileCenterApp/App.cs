using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobileCenterApi;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class App : Application
	{
		MobileCenterAPIServiceApiKeyApi api;
		public App ()
		{
			api = new MobileCenterAPIServiceApiKeyApi("MobileCenter","foo");
			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						},
						CreateButton("Login",async ()=>{
							var apps = await api.GetApps();
							Console.WriteLine(apps);
						}),

					}
				}
			};
		}

		Button CreateButton(string text, Action tapped)
		{
			var button = new Button
			{
				Text = text,
			};
			button.Clicked += (sender, e) => tapped?.Invoke();;
			return button;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
