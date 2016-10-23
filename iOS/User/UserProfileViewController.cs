using Foundation;
using System;
using UIKit;

namespace Shoperella.iOS
{
	public partial class UserProfileViewController : BaseViewController
	{
		public UserProfileViewController(IntPtr handle) : base (handle)
        {
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this);

			if (AppDelegate.User != null)
			{
				lblName.Text = AppDelegate.User.name;
				lblEmail.Text = AppDelegate.User.email;
				img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.User.photoURL)));
				SetRImage(img);
			}

			btnLogout.TouchUpInside += delegate
			{
				Settings.SetUserJSON(null);
				ViewController.Instance.HandleLogout();
			};

			if (AppDelegate.Vendor == null)
			{
				btnCreateVendor.TouchUpInside += delegate
				{
					var registerVendorVC = Storyboard.InstantiateViewController("VendorRegisterViewController") as VendorRegisterViewController;
					NavigationController.PushViewController(registerVendorVC, true);
				};
			}
			else
			{
				btnCreateVendor.SetTitle("SWITCH TO VENDOR", UIControlState.Normal);
				btnCreateVendor.TouchUpInside += delegate
				{
					ViewController.Instance.HandleContextSwitch(Settings.CONTEXT_VENDOR);
				};
			}
		}

		partial void GoToEditProfile(UIButton sender)
		{
			var editProfileVC = Storyboard.InstantiateViewController("UserProfileEditViewController") as UserProfileEditViewController;
			NavigationController.PushViewController(editProfileVC, true);
		}

		//void HandleLoginManagerRequestTokenHandler(LoginManagerLoginResult result, Foundation.NSError error)
		//{
		//	if (result.IsCancelled)
		//		return;
		//	var token = result.Token.TokenString;
		//	var userId = result.Token.UserID;
		//	Settings.FacebookToken = token;
		//	Settings.FacebookUID = userId;
		//	var apiClient = new API.Client();
		//	var userJson = apiClient.Login(token, userId);
		//	AppDelegate.User = Settings.SetUserJSON(userJson);

		//	lblName.Text = AppDelegate.User.name;
		//	lblSignupDate.Text = "since " + AppDelegate.User.created;
		//	img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.User.photoURL)));
		//}
	}
}

