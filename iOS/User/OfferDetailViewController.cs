using Foundation;
using System;
using UIKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SDWebImage;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class OfferDetailViewController : BaseViewController
	{
		public Offer offer { get; set; }

		public OfferDetailViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this, showMenu: false);

			if (offer.Vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(offer.Vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblTitle.Text = offer.Title;
			lblCaption.Text = offer.Caption;
			txtDescription.Text = offer.Description;

			System.Timers.Timer timeTimer = new System.Timers.Timer()
			{
				Interval = 1000,
				AutoReset = true
			};
			timeTimer.Elapsed += TimeTimer_Elapsed;
			timeTimer.Start();
		}

		void TimeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			DateTime date = offer.ExpiresAt != null ? offer.ExpiresAt : new DateTime(2016, 09, 25, 20, 30, 20);
			var timeRemaining = date.Subtract(DateTime.Now);
			InvokeOnMainThread(delegate
			{
				lblTimeRemain.Text = timeRemaining.Hours.ToString("00") + ":" + timeRemaining.Minutes.ToString("00") + ":" + timeRemaining.Seconds.ToString("00");
			});
		}

		async partial void OnClickRedeem(UIButton sender)
		{
			var client = new Client();

			ShowLoadingView("Processing...");

			var reOffer = await client.RedeemOffer(offer.ID, AppDelegate.User);
			if (reOffer == null)
			{
				HideLoadingView();
				ShowMessageBox(null, "Failed...");
				return;
			}

			HideLoadingView();
			ShowMessageBox(null, "Redeemed!");
			NavigationController.PopViewController(true);
		}

		partial void OnClickNoThanks(UIButton sender)
		{
			NavigationController.PopViewController(true);
		}
	}
}