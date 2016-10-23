using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content;
using Android.Runtime;
using System;

using Xamarin.Facebook;
using Xamarin.Facebook.Login;

using Shoperella.API;
using Shoperella.Model;

[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]

namespace Shoperella.Droid
{
	[Activity(Label = "Shoperella", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
	public class LoginActivity : Activity
	{
		static readonly string[] PERMISSIONS = new[] { "public_profile", "email" };
		ICallbackManager callbackManager;
				
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Login);

			#region init FB
			FacebookSdk.SdkInitialize(this.ApplicationContext);

			callbackManager = CallbackManagerFactory.Create();

			var loginCallback = new FacebookCallback<LoginResult>
			{
				HandleSuccess = loginResult =>
				{
					var accessToken = AccessToken.CurrentAccessToken;
					LoginShoperella(accessToken.Token, accessToken.UserId);
				},
				HandleCancel = () =>
				{

				},
				HandleError = loginError =>
				{

				}
			};

			LoginManager.Instance.RegisterCallback(callbackManager, loginCallback);
			#endregion


			Typeface tf = Typeface.CreateFromAsset(Assets, "Fonts/NexaBold.otf");

			Button btnGetStarted = FindViewById<Button>(Resource.Id.btnGetStarted);
			btnGetStarted.SetTypeface(tf, TypefaceStyle.Normal);

			Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
			btnLogin.SetTypeface(tf, TypefaceStyle.Normal);

			btnGetStarted.Click += OnClickGetStarted;
			btnLogin.Click += OnClickLogin;
		}

		private void LoginShoperella(string token, string userId)
		{
			try
			{
				var apiClient = new Client();
				var userJson = apiClient.Login(token, userId);

				if (userJson == null)
				{
					//ShowMessageBox(null, "register failed");
					return;
				}
				//AppDelegate.User = new User(JObject.Parse(userJson));
				//Settings.SetUserJSON(userJson);
				Settings.SetUserJSON(userJson);

				//SetUserDetail(AppDelegate.User);

				var UserMainAC = new Intent(this, typeof(UserMainActivity));
				StartActivity(UserMainAC);
			}
			catch (Exception ex)
			{
			}
		}

		private void OnClickLogin(object sender, EventArgs e)
		{
			LoginManager.Instance.LogInWithReadPermissions(this, PERMISSIONS);
			//LoginManager.Instance.LogOut();
		}
		private void OnClickGetStarted(object sender, EventArgs e)
		{
			var UserMainAC = new Intent(this, typeof(UserMainActivity));
			StartActivity(UserMainAC);
		}

		#region FB login callback
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
		}

		class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
		{
			public Action HandleCancel { get; set; }
			public Action<FacebookException> HandleError { get; set; }
			public Action<TResult> HandleSuccess { get; set; }

			public void OnCancel()
			{
				var c = HandleCancel;
				if (c != null)
					c();
			}

			public void OnError(FacebookException error)
			{
				var c = HandleError;
				if (c != null)
					c(error);
			}

			public void OnSuccess(Java.Lang.Object result)
			{
				var c = HandleSuccess;
				if (c != null)
					c(result.JavaCast<TResult>());
			}
		}
		#endregion
	}
}


