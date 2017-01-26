using System;
using System.Collections.Generic;
using SimpleAuth;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public partial class LoginPage : ContentPage
	{

		IBasicAuthenicator authenticator;
		public LoginPage(IBasicAuthenicator authenticator)
		{
			this.authenticator = authenticator;
			InitializeComponent();
			this.Title = "Login";
			this.ToolbarItems.Add(new ToolbarItem("Cancel", null, () =>
			{
				authenticator.OnCancelled();
			}));
		}


		void UsernameCompleted(object sender, System.EventArgs e)
		{
			Password.Focus();
		}
		void PasswordCompleted(object sender, System.EventArgs e)
		{
			Login();
		}
		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			Login();
		}

		async void Login()
		{
			if (string.IsNullOrWhiteSpace(Username.Text))
			{
				await this.DisplayAlert("Error", "Username is invalid", "Ok");
				return;
			}

			if (string.IsNullOrWhiteSpace(Password.Text))
			{
				await this.DisplayAlert("Error", "Password is invalid", "Ok");
				return;
			}

			try
			{
				bool success = await authenticator.VerifyCredentials(Username.Text, Password.Text);

				if (success)
					await this.Navigation.PopModalAsync();
				else
					await this.DisplayAlert("Error", "Invalid credentials", "Ok");

			}
			catch (Exception ex)
			{
				await this.DisplayAlert("Error", ex.Message, "Ok");
			}
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			Username.Focus();
		}
		protected override bool OnBackButtonPressed()
		{
			authenticator.OnCancelled();
			return base.OnBackButtonPressed();
		}
	}
}
