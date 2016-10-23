using Foundation;
using System;
using UIKit;

namespace Shoperella.iOS
{
	public partial class VendorProfileViewController : BaseViewController
	{
		public VendorProfileViewController(IntPtr handle) : base (handle)
        {
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this);
			SetRImage(img);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			if (AppDelegate.Vendor != null)
			{
				lblName.Text = AppDelegate.Vendor.Name;

				if (AppDelegate.Vendor != null)
				{
					if (AppDelegate.Vendor.ImagePath != null)
						img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.Vendor.ImagePath)));
				}
			}
		}

		partial void GoToEditProfile(UIButton sender)
		{
			var editProfileVC = Storyboard.InstantiateViewController("VendorProfileEditViewController") as VendorProfileEditViewController;
			NavigationController.PushViewController(editProfileVC, true);
		}

		partial void OnClickSwitchToUser(UIButton sender)
		{
			ViewController.Instance.HandleContextSwitch(Settings.CONTEXT_USER);
		}
	}
}

