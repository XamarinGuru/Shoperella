using System;

using Foundation;
using UIKit;

using SDWebImage;

using Shoperella.Model;
using Shoperella.API;

namespace Shoperella.iOS
{
	public partial class VHOpenOffersCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("VHOpenOffersCell");
		public static readonly UINib Nib;

		private VendorHomeViewController rootVC;

		static VHOpenOffersCell()
		{
			Nib = UINib.FromName("VHOpenOffersCell", NSBundle.MainBundle);
		}

		public VHOpenOffersCell(IntPtr handle) : base(handle)
		{
		}

		public void SetCell(Offer offer, VendorHomeViewController rootVC)
		{
			this.rootVC = rootVC;

			var vendor = AppDelegate.Vendor;

			if (vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblVendorName.Text = vendor.Name;
			lblExpireDate.Text = offer.ExpiresAt.ToString();
			lblDealName.Text = offer.Title;
			lblDealCaption.Text = offer.Caption;
			btnView.Tag = int.Parse(offer.ID);
		}

		protected void SetRCellBorder(UIImageView image)
		{

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
			//var createOfferVC = rootVC.Storyboard.InstantiateViewController("CreateOfferViewController") as CreateOfferViewController;
			//createOfferVC.wishID = sender.Tag.ToString();
			//rootVC.NavigationController.PushViewController(createOfferVC, true);
		}


	}
}
