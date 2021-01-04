﻿using System;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NathanFirebaseAuth
{
	public class LoginPage : ContentPage
	{
        private readonly Entry emailEntry;
        private readonly Entry passwordEntry;

        public LoginPage()
		{
			Title = "Login";

			var toolbarItem = new ToolbarItem
			{
				Text = "Sign Up"
			};
			toolbarItem.Clicked += OnSignUpButtonClicked;
			ToolbarItems.Add(toolbarItem);

			emailEntry = new Entry
			{
				Placeholder = "Email"
			};
			passwordEntry = new Entry
			{
				IsPassword = true,
				Placeholder = "Password"

			};
			var loginButton = new Button
			{
				Text = "Login"
			};
			loginButton.Clicked += OnLoginButtonClicked;

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = {
					new Label { Text = "Email" },
					emailEntry,
					new Label { Text = "Password" },
					passwordEntry,
					loginButton,
				}
			};
		}

		async void OnSignUpButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SignUpPage());
		}

		async void OnLoginButtonClicked(object sender, EventArgs e)
		{
			var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.WebAPIkey));
			try
			{
				var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text, passwordEntry.Text);
				var content = await auth.GetFreshAuthAsync();
				var serializedcontent = JsonConvert.SerializeObject(content);
				Preferences.Set("MyFirebaseRefreshToken", serializedcontent);
				await Navigation.PushAsync(new DashboardPage());
			}
			catch (Exception)
			{
				await Application.Current.MainPage.DisplayAlert("Alert", "Invalid useremail or password", "OK");
			}
		}
	}
}


