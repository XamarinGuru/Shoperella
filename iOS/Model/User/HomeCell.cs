using System;

using Foundation;
using UIKit;

using SDWebImage;

using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class HomeCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("HomeCell");
		public static readonly UINib Nib;

		private HomeViewController rootVC;
		private string type;
		private Deal deal = null;
		private Offer offer = null;

		static HomeCell()
		{
			Nib = UINib.FromName("HomeCell", NSBundle.MainBundle);
		}

		protected HomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void SetCell(Offer offer, HomeViewController rootVC)
		{
			type = "offer";
			this.offer = offer;
			this.rootVC = rootVC;

			if (offer.Vendor != null && offer.Vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(offer.Vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblVendorName.Text = offer.Vendor.Name;
			lblExpireDate.Text = offer.ExpiresAt.ToString();
			lblDealName.Text = offer.Title;
			lblDealCaption.Text = offer.Caption;
		}
		public void SetCell(Deal deal, HomeViewController rootVC)
		{
			type = "deal";
			this.deal = deal;
			this.rootVC = rootVC;

			if (deal.Vendor != null && deal.Vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(deal.Vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblVendorName.Text = deal.Vendor.Name;
			lblExpireDate.Text = deal.ExpiresAt.ToString();
			lblDealName.Text = deal.Title;
			lblDealCaption.Text = deal.Caption;
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
			if (type == "offer")
			{
				OfferDetailViewController offerDetailVC = rootVC.Storyboard.InstantiateViewController("OfferDetailViewController") as OfferDetailViewController;
				offerDetailVC.offer = offer;
				rootVC.NavigationController.PushViewController(offerDetailVC, true);
			}

			if (type == "deal")
			{
				DealDetailViewController dealDetailVC = rootVC.Storyboard.InstantiateViewController("DealDetailViewController") as DealDetailViewController;
				dealDetailVC.deal = deal;
				rootVC.NavigationController.PushViewController(dealDetailVC, true);
			}
		}
	}
}
