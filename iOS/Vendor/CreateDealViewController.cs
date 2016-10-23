using Foundation;
using System;
using UIKit;

namespace Shoperella.iOS
{
	public partial class CreateDealViewController : BaseViewController
	{
		public CreateDealViewController(IntPtr handle) : base (handle)
        {
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this);

			SetupTimePicker(txtExpire, UIDatePickerMode.DateAndTime, "{0: MM/dd/yyyy hh:mm tt}", futureDatesOnly: true);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			if (AppDelegate.Vendor != null)
			{
				if (AppDelegate.Vendor.ImagePath != null)
					img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.Vendor.ImagePath)));
			}
		}

		partial void OnClickCreateTemplate(UIButton sender)
		{
			throw new NotImplementedException();
		}

		//partial void OnClickCreateDeal(UIButton sender)
		//{
		//	if (!ValidateData())
		//	{
		//		ShowMessageBox(null, "please specify all fields");
		//		return;
		//	}

		//	var client = new API.Client();

		//	var titleStr = txtTitle.Text;
		//	var captionStr = txtCaption.Text;
		//	var descStr = txtDescription.Text;
		//	var expiresStr = ConvertISODate(txtExpire.Text);

		//	System.Threading.ThreadPool.QueueUserWorkItem(delegate
		//	{
		//		var deal = client.CreateDeal(AppDelegate.Vendor, titleStr, captionStr, descStr, expiresStr, AppDelegate.User);
		//		if (deal == null)
		//		{
		//			ShowMessageBox(null, "register failed");
		//			return;
		//		}
		//		AppDelegate.Vendor.Deals.Add(deal);
		//		//InvokeOnMainThread(delegate
		//		//{
		//		//	NavigationController.PopViewController(true);
		//		//});
		//		ShowMessageBox(null, "register success!");
		//	});
		//}

		private bool ValidateData()
		{
			if (txtTitle.Text.Equals(string.Empty) || txtCaption.Text.Equals(string.Empty) || txtDescription.Text.Equals(string.Empty) || txtExpire.Text.Equals(string.Empty))
				return false;
			return true;
		}


	}
}

