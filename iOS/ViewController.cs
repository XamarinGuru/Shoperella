using System;
using UIKit;
using CoreLocation;
using Facebook.LoginKit;
using Newtonsoft.Json.Linq;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class ViewController : BaseViewController
	{
		static readonly string[] PERMISSIONS = new[] { "public_profile", "email" };

		public static ViewController Instance;

		CLLocationManager locationManager = new CLLocationManager();
		Navigation nav;

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Instance = this;

			SetRButton(btnLogin);
			SetRButton(btnRegister);
			// if we already have a user then skip the splash and go straight into the full app
			if (AppDelegate.User != null)
			{
				SetUserDetail(AppDelegate.User);

				nav = new Navigation(Storyboard);
				View.AddSubview(Navigation.Instance.View);
			}

			GetLocation();
		}

		partial void OnClickLogin(UIButton sender)
		{
			var facebookLoginManager = new Facebook.LoginKit.LoginManager();
			facebookLoginManager.LogOut();
			facebookLoginManager.LogInWithReadPermissions(PERMISSIONS, ViewController.Instance, HandleLoginManagerRequestTokenHandler);
		}

		partial void OnClickRegister(UIButton sender)
		{
			var facebookLoginManager = new Facebook.LoginKit.LoginManager();
			facebookLoginManager.LogOut();
			facebookLoginManager.LogInWithReadPermissions(PERMISSIONS, this, HandleLoginManagerRequestTokenHandler);
		}

		void HandleLoginManagerRequestTokenHandler(LoginManagerLoginResult result, Foundation.NSError error)
		{
			if (result.IsCancelled)
				return;
			var token = result.Token.TokenString;
			var userId = result.Token.UserID;
			Settings.FacebookToken = token;
			Settings.FacebookUID = userId;
			var apiClient = new Client();
			var userJson = apiClient.Login(token, userId);

			if (userJson == null)
			{
				ShowMessageBox(null, "register failed");
				return;
			}
			AppDelegate.User = new User(JObject.Parse(userJson));
			Settings.SetUserJSON(userJson);

			SetUserDetail(AppDelegate.User);

			nav = new Navigation(Storyboard);
			View.AddSubview(Navigation.Instance.View);
		}

		//void HandleRegisterLoginManagerRequestTokenHandler(LoginManagerLoginResult result, Foundation.NSError err)
		//{
		//	if (result.IsCancelled)
		//		return;
		//	var token = result.Token.TokenString;
		//	var userId = result.Token.UserID;
		//	Settings.FacebookToken = token;
		//	Settings.FacebookUID = userId;
		//	var apiClient = new Client();
		//	var userJson = apiClient.Login(token, userId);
		//	AppDelegate.User = Settings.SetUserJSON(userJson);

		//	var registerVendorVC = Storyboard.InstantiateViewController("VendorRegisterViewController") as VendorRegisterViewController;
		//	this.View.AddSubview(registerVendorVC.View);
		//}



		private void GetLocation()
		{
			locationManager.RequestWhenInUseAuthorization();
			locationManager.DesiredAccuracy = 2000;
			if (CLLocationManager.LocationServicesEnabled)
			{
				locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
				{
					var location = e.Locations[0];
					AppDelegate.Latitude = location.Coordinate.Latitude;
					AppDelegate.Longitude = location.Coordinate.Longitude;
					Settings.Latitude = location.Coordinate.Latitude;
					Settings.Longitude = location.Coordinate.Longitude;
					locationManager.StopUpdatingLocation();
				};
				locationManager.RequestLocation();
			}
		}

		public void HandleLogout() 
		{
			Navigation.Instance.View.RemoveFromSuperview();
		}

		public void HandleContextSwitch(string targetContext)
		{
			InvokeOnMainThread(delegate
			{
				AppDelegate.CurrentContext = targetContext;
				Settings.CurrentContext = targetContext;
				Navigation.Instance.View.RemoveFromSuperview();
				nav = new Navigation(Storyboard);
				View.AddSubview(Navigation.Instance.View);
			});
		}


	}
}
