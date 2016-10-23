using System;

using Foundation;
using UIKit;

using SDWebImage;

using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class OffersCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("OffersCell");
		public static readonly UINib Nib;

		private OffersViewController rootVC;
		private Action<string> callback;
		private Offer offer;

		static OffersCell()
		{
			Nib = UINib.FromName("OffersCell", NSBundle.MainBundle);
		}

		public OffersCell(IntPtr handle) : base(handle)
		{
		}

		public void SetCell(Offer offer, Action<string> callback, OffersViewController rootVC)
		{
			this.offer = offer;
			this.callback = callback;
			this.rootVC = rootVC;

			if (offer.Vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(offer.Vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblTitle.Text = offer.Title;
			lblCaption.Text = offer.Caption;
			btnView.Tag = int.Parse(offer.ID);
			btnX.Tag = int.Parse(offer.ID);
		}

		override public void LayoutSubviews()
		{
			base.LayoutSubviews();

			img.LayoutIfNeeded();
			img.Layer.CornerRadius = img.Frame.Size.Width / 2;
			img.Layer.BorderWidth = 1;
			img.Layer.BorderColor = UIColor.Gray.CGColor;
			img.Layer.MasksToBounds = true;

			imgBorder.LayoutIfNeeded();
			imgBorder.Layer.CornerRadius = 4;
			imgBorder.Layer.MasksToBounds = true;

			btnView.LayoutIfNeeded();
			btnView.Layer.CornerRadius = 3;
			btnView.Layer.MasksToBounds = true;
		}

		partial void OnClickView(UIButton sender)
		{
			OfferDetailViewController offerDetailVC = rootVC.Storyboard.InstantiateViewController("OfferDetailViewController") as OfferDetailViewController;
			offerDetailVC.offer = offer;
			rootVC.NavigationController.PushViewController(offerDetailVC, true);
		}

		partial void OnClickX(UIButton sender)
		{
			var offerID = sender.Tag.ToString();
			callback(offerID);
		}
	}
}
