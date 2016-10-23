using Foundation;
using System;
using UIKit;

namespace Shoperella.iOS
{
	public partial class UserProfileEditViewController : BaseViewController
	{
		public UserProfileEditViewController(IntPtr handle) : base (handle)
        {
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var tap = new UITapGestureRecognizer(() => { View.EndEditing(true); });
			View.AddGestureRecognizer(tap);

			HeaderStyleGenerator.Generate(this, showMenu: false);

			if (AppDelegate.User != null)
			{
				//lblName.Text = AppDelegate.User.name;
				//lblSignupDate.Text = "since " + AppDelegate.User.created;
				img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.User.photoURL)));
				SetRImage(img);
			}

			//btnLogout.TouchUpInside += delegate
			//{
			//	Settings.SetUserJSON(null);
			//	ViewController.Instance.HandleLogout();
			//};

			//if (AppDelegate.Vendor == null)
			//{
			//	btnCreateVendor.TouchUpInside += delegate
			//	{
			//		var registerVendorVC = Storyboard.InstantiateViewController("VendorRegisterViewController") as VendorRegisterViewController;
			//		NavigationController.PushViewController(registerVendorVC, true);
			//	};
			//}
			//else if (AppDelegate.CurrentContext == Settings.CONTEXT_USER)
			//{
			//	btnCreateVendor.SetTitle("SWITCH TO VENDOR", UIControlState.Normal);
			//	btnCreateVendor.TouchUpInside += delegate
			//	{
			//		ViewController.Instance.HandleContextSwitch(Settings.CONTEXT_VENDOR);
			//	};
			//}
			//else if (AppDelegate.CurrentContext == Settings.CONTEXT_VENDOR)
			//{
			//	btnCreateVendor.SetTitle("SWITCH TO USER", UIControlState.Normal);
			//	btnCreateVendor.TouchUpInside += delegate
			//	{
			//		ViewController.Instance.HandleContextSwitch(Settings.CONTEXT_USER);
			//	};
			//}


		}
	}
}

