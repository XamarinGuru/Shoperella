using Foundation;
using System;
using UIKit;

using Newtonsoft.Json.Linq;

using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class VendorRegisterViewController : BaseViewController
    {
		private Byte[] mBArrLogo = null;

		UIImagePickerController imagePicker = new UIImagePickerController();

		public VendorRegisterViewController (IntPtr handle) : base(handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var tap = new UITapGestureRecognizer(() => { View.EndEditing(true); });
			View.AddGestureRecognizer(tap);

			HeaderStyleGenerator.Generate(this, showMenu: false);

			SetRImage(img);

			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
			imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
			imagePicker.Canceled += Handle_Canceled;
		}

		partial void OnClickChangeImage(UIButton sender)
		{
			this.PresentViewController(imagePicker, true, null);
		}

		partial void OnClickChangeLocation(UIButton sender)
		{
			var pickupLocationVC = Storyboard.InstantiateViewController("LocationPickUpViewController") as LocationPickUpViewController;
			pickupLocationVC.traDelegate = ReceiveLocationInfoCallBack;
			NavigationController.PushViewController(pickupLocationVC, true);
		}
		public void ReceiveLocationInfoCallBack(PlaceDetailsAPI_RootObject locationInfo)
		{
			var formatedAddressInfo = GetFormatedAddressInfo(locationInfo);

			txtLocation.Text = locationInfo.result.formatted_address;
			txtCity.Text = formatedAddressInfo["city"];
			txtStreet.Text = formatedAddressInfo["street"];
			txtZip.Text = formatedAddressInfo["zip"];
		}

		private bool VerifyVendorData()
		{
			if (txtName.Text.Equals(string.Empty) || txtCity.Text.Equals(string.Empty) || txtStreet.Text.Equals(string.Empty) || txtZip.Text.Equals(string.Empty))
			{
				ShowMessageBox(null, "Please specify valide vendor data");
				return false;
			}
			return true;
		}

		async partial void OnClickCreateVendor(UIButton sender)
		{
			if (VerifyVendorData())
			{
				var client = new API.Client();

				var name = txtName.Text;
				var city = txtCity.Text;
				var street = txtStreet.Text;
				var zip = txtZip.Text;

				ShowLoadingView("Creating Vendor...");

				var vendorJson = await client.CreateVendor(mBArrLogo, name, city, street, zip, AppDelegate.User);

				HideLoadingView();

				try
				{
					AppDelegate.Vendor = new Vendor(JObject.Parse(vendorJson));
					Settings.SetVendorJSON(vendorJson);

					ViewController.Instance.HandleContextSwitch(Settings.CONTEXT_VENDOR);
				}
				catch (Exception ex)
				{
					ShowMessageBox(null, "register failed");
					Console.WriteLine("Exception in update vendor profile:" + ex.Message);
				}
			}
		}

		protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
		{
			var originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
			img.Image = originalImage;

			try
			{
				using (NSData imageData = MaxResizeImage(originalImage, 90f, 90f).AsPNG())
				{
					mBArrLogo = new Byte[imageData.Length];
					System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, mBArrLogo, 0, Convert.ToInt32(imageData.Length));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception in update vendor profile:" + ex.Message);
			}

			imagePicker.DismissViewControllerAsync(true);
		}

		void Handle_Canceled(object sender, EventArgs e)
		{
			imagePicker.DismissViewControllerAsync(true);
		}
    }
}