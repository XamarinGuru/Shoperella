using Foundation;
using System;
using UIKit;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class CreateTemplateViewController : BaseViewController
	{
		public string wishID;
		public string templateType;

		Client client = new Client();

		string titleStr;
		string captionStr;
		string descStr;
		string expiresStr;

		private Action CreateProcessByType;

		public CreateTemplateViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var tap = new UITapGestureRecognizer(() => { View.EndEditing(true); });
			View.AddGestureRecognizer(tap);

			HeaderStyleGenerator.Generate(this, showMenu:false);

			if (AppDelegate.Vendor != null)
			{
				if (AppDelegate.Vendor.ImagePath != null)
					img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.Vendor.ImagePath)));
			}

			switch (templateType)
			{
				case "deal":
					btnSubmit.SetTitle("CREATE DEAL", UIControlState.Normal);
					CreateProcessByType = CreateDeal;
					break;
				case "offer":
					btnSubmit.SetTitle("CREATE OFFER", UIControlState.Normal);
					CreateProcessByType = CreateOffer;
					break;
				case "autoresponse":
					btnSubmit.SetTitle("CREATE AUTO RESPONSE", UIControlState.Normal);
					CreateProcessByType = CreateAutoResponse;
					break;
				default:
					break;
			}

			SetupTimePicker(txtExpire, UIDatePickerMode.DateAndTime, "{0: MM/dd/yyyy hh:mm tt}", futureDatesOnly: true);
		}

		partial void OnClickCreateTemplate(UIButton sender)
		{
			if (!ValidateData())
			{
				ShowMessageBox(null, "please specify all fields");
				return;
			}

			titleStr = txtTitle.Text;
			captionStr = txtCaption.Text;
			descStr = txtDescription.Text;
			expiresStr = ConvertISODate(txtExpire.Text);

			CreateProcessByType();
		}

		private void CreateDeal()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Processing...");

				var deal = client.CreateDeal(AppDelegate.Vendor, titleStr, captionStr, descStr, expiresStr, AppDelegate.User);

				HideLoadingView();

				if (deal == null)
				{
					ShowMessageBox(null, "Failed");
					return;
				}
				InvokeOnMainThread(delegate
				{
					NavigationController.PopViewController(true);
				});
			});
		}

		private void CreateOffer()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Processing...");

				var offer = client.CreateOffer(AppDelegate.Vendor.ID, wishID, titleStr, captionStr, descStr, expiresStr, AppDelegate.User);

				HideLoadingView();

				if (offer == null)
				{
					ShowMessageBox(null, "Failed");
					return;
				}
				InvokeOnMainThread(delegate
				{
					NavigationController.PopViewController(true);
				});
			});
		}

		private void CreateAutoResponse()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Processing...");

				var autoResponse = client.CreateAutoResponse(AppDelegate.Vendor, titleStr, captionStr, descStr, expiresStr, AppDelegate.User);

				HideLoadingView();

				if (autoResponse == null)
				{
					ShowMessageBox(null, "Failed");
					return;
				}
				InvokeOnMainThread(delegate
				{
					HideLoadingView();
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

