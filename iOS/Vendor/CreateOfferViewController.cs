using System;
using UIKit;
using Foundation;

namespace Shoperella.iOS
{
	public partial class CreateOfferViewController : BaseViewController
	{
		public string wishID;
		public CreateOfferViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this, showMenu:false);

			if (AppDelegate.Vendor != null)
			{
				if (AppDelegate.Vendor.ImagePath != null)
					img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.Vendor.ImagePath)));
			}

			SetupTimePicker(txtExpire, UIDatePickerMode.DateAndTime, "{0: MM/dd/yyyy hh:mm tt}", futureDatesOnly: true);
		}

		partial void OnClickCreateDeal(UIButton sender)
		{
			if (!ValidateData())
			{
				ShowMessageBox(null, "please specify all fields");
				return;
			}
			var client = new API.Client();
			var titleStr = txtTitle.Text;
			var captionStr = txtCaption.Text;
			var descStr = txtDescription.Text;
			var expiresStr = ConvertISODate(txtExpire.Text);

			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var offer = client.CreateOffer(AppDelegate.Vendor.ID, wishID, titleStr, captionStr, descStr, expiresStr, AppDelegate.User);
				if (offer == null)
				{
					ShowMessageBox(null, "register offer failed");
					return;
				}
				ShowMessageBox(null, "register success!");

				InvokeOnMainThread(delegate
				{
					NavigationController.PopViewController(true);
				});
			});
		}

		private bool ValidateData()
		{
			if (txtTitle.Text.Equals(string.Empty) || txtCaption.Text.Equals(string.Empty) || txtDescription.Text.Equals(string.Empty) || txtExpire.Text.Equals(string.Empty))
				return false;
			return true;
		}
	}
}

