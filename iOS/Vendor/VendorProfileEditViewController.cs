using Foundation;
using System;
using UIKit;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class VendorProfileEditViewController : BaseViewController
	{
		private Byte[] mBArrLogo = null;

		UIImagePickerController imagePicker = new UIImagePickerController();

		public VendorProfileEditViewController(IntPtr handle) : base (handle)
        {
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var tap = new UITapGestureRecognizer(() => { View.EndEditing(true); });
			View.AddGestureRecognizer(tap);

			HeaderStyleGenerator.Generate(this, showMenu: false);

			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
			imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
			imagePicker.Canceled += Handle_Canceled;

			SetVendorProfile();
		}

		private void SetVendorProfile()
		{
			if (AppDelegate.Vendor != null)
			{
				if (AppDelegate.Vendor.ImagePath != null)
					img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.Vendor.ImagePath)));

				SetRImage(img);
			}

			lblName.Text = AppDelegate.Vendor.Name;
			lblAddress.Text = AppDelegate.Vendor.Address;
			lblCity.Text = AppDelegate.Vendor.City;
			lblST.Text = AppDelegate.Vendor.State;
			lblZip.Text = AppDelegate.Vendor.Zip;
		}


		partial void OnClickChangeImage(UIButton sender)
		{
			this.PresentViewController(imagePicker, true, null);
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

		partial void OnClickChangeLocation(UIButton sender)
		{
			var pickupLocationVC = Storyboard.InstantiateViewController("LocationPickUpViewController") as LocationPickUpViewController;
			pickupLocationVC.traDelegate = ReceiveLocationInfoCallBack;
			NavigationController.PushViewController(pickupLocationVC, true);
		}
		public void ReceiveLocationInfoCallBack(PlaceDetailsAPI_RootObject locationInfo)
		{
			var formatedAddressInfo = GetFormatedAddressInfo(locationInfo);

			lblAddress.Text = locationInfo.result.formatted_address;
			lblCity.Text = formatedAddressInfo["city"];
			lblST.Text = formatedAddressInfo["street"];
			lblZip.Text = formatedAddressInfo["zip"];
		}

		private bool VerifyVendorData()
		{
			if (lblName.Text.Equals(string.Empty) || lblCity.Text.Equals(string.Empty) || lblST.Text.Equals(string.Empty) || lblZip.Text.Equals(string.Empty))
			{
				ShowMessageBox(null, "Please specify valide vendor data");
				return false;
			}
			return true;
		}

		async partial void OnClickSaveProfile(UIButton sender)
		{
			if (VerifyVendorData())
			{
				ShowLoadingView("Updating Retailer Profile...");
				var client = new API.Client();
				var id = AppDelegate.Vendor.ID;
				var name = lblName.Text;
				var city = lblCity.Text;
				var street = lblST.Text;
				var zip = lblZip.Text;

				var vendorJson = await client.UpdateVendor(id, mBArrLogo, name, city, street, zip, AppDelegate.User);

				try
				{
					AppDelegate.Vendor = new Vendor(JObject.Parse(vendorJson));
					Settings.SetVendorJSON(vendorJson);

					AppDelegate.NavHeaderView.SetName("Hey, " + AppDelegate.Vendor.Name);
					if (AppDelegate.Vendor.ImagePath != null)
						AppDelegate.NavHeaderView.SetImage(AppDelegate.Vendor.ImagePath);

					this.NavigationController.PopViewController(true);

					HideLoadingView();
				}
				catch (Exception ex)
				{
					HideLoadingView();
					ShowMessageBox(null, "update failed");
					Console.WriteLine("Exception in update vendor profile:" + ex.Message);
				}
			}
		}

		partial void OnClickAutoResponse(UIButton sender)
		{
			var createTemplateVC = Storyboard.InstantiateViewController("CreateTemplateViewController") as CreateTemplateViewController;
			createTemplateVC.templateType = "autoresponse";
			NavigationController.PushViewController(createTemplateVC, true);
		}
	}
}

