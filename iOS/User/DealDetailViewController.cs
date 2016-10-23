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
	public partial class DealDetailViewController : BaseViewController
    {
		public Deal deal { get; set; }

        public DealDetailViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this, showMenu:false);

			if (deal.Vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(deal.Vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblTitle.Text = deal.Title;
			lblCaption.Text = deal.Caption;
			txtDescription.Text = deal.Description;

			System.Timers.Timer timeTimer = new System.Timers.Timer() { 
				Interval = 1000,
				AutoReset = true
			};
			timeTimer.Elapsed += TimeTimer_Elapsed;
			timeTimer.Start();
		}

		void TimeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			DateTime date = deal.ExpiresAt != null ? deal.ExpiresAt : new DateTime(2016, 09, 25, 20, 30, 20);
			var timeRemaining = date.Subtract(DateTime.Now);
			InvokeOnMainThread(delegate {
				lblTimeRemain.Text = timeRemaining.Hours.ToString("00") + ":" + timeRemaining.Minutes.ToString("00") + ":" + timeRemaining.Seconds.ToString("00");
			});
		}

		async partial void OnClickAddToFavorite(UIButton sender)
		{
			var client = new Client();

			ShowLoadingView("Saving to favorite...");

			var deals = await client.AddToFavorite(deal.ID, AppDelegate.User);
			if (deals == null)
				ShowMessageBox(null, "Already saved to favorite...");

			HideLoadingView();

			new UIAlertView("You Faved This Deal", "You can access your favorite deals at any time in the menu", null, "Got It", null).Show();
			btnFavorites.SetTitle("Favorited!", UIControlState.Normal);
		}

		async partial void OnClickRedeem(UIButton sender)
		{
			var client = new Client();

			ShowLoadingView("Processing...");

			var reDeal = await client.RedeemDeal(deal.ID, AppDelegate.User);
			if (reDeal == null)
			{
				HideLoadingView();
				ShowMessageBox(null, "Failed...");
				return;
			}

			HideLoadingView();
			ShowMessageBox(null, "Redeemed!");
			NavigationController.PopViewController(true);
		}

		partial void OnClickRunningLate(UIButton sender)
		{
			//throw new NotImplementedException();
		}

		partial void OnClickNoThanks(UIButton sender)
		{
			NavigationController.PopViewController(true);
		}
	}
}