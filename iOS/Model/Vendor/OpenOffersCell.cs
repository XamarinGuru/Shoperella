using System;

using Foundation;
using UIKit;

using SDWebImage;

using Shoperella.Model;
using Shoperella.API;

namespace Shoperella.iOS
{
	public partial class OpenOffersCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("OpenOffersCell");
		public static readonly UINib Nib;

		private Action<string> RenewCallBack;
		private Action<string> RemoveCallBack;

		static OpenOffersCell()
		{
			Nib = UINib.FromName("OpenOffersCell", NSBundle.MainBundle);
		}

		public OpenOffersCell(IntPtr handle) : base(handle)
		{
		}

		public void SetCell(Offer offer, Action<string> RenewCallBack, Action<string> RemoveCallBack)
		{
			this.RenewCallBack = RenewCallBack;
			this.RemoveCallBack = RemoveCallBack;

			var vendor = AppDelegate.Vendor;

			if (vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblTitle.Text = offer.Title;
			lblCaption.Text = offer.Caption;
			btnRemoveOffer.Tag = int.Parse(offer.ID);
			btnRenewOffer.Tag = int.Parse(offer.ID);
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

			btnRemoveOffer.LayoutIfNeeded();
			btnRemoveOffer.Layer.CornerRadius = 3;
			btnRemoveOffer.Layer.MasksToBounds = true;

			btnRenewOffer.LayoutIfNeeded();
			btnRenewOffer.Layer.CornerRadius = 3;
			btnRenewOffer.Layer.MasksToBounds = true;
		}

		partial void OnClickRenewOffer(UIButton sender)
		{
			var offerID = sender.Tag.ToString();
			RenewCallBack(offerID);
		}

		partial void OnClickRemoveOffer(UIButton sender)
		{
			var offerID = sender.Tag.ToString();
			RemoveCallBack(offerID);
		}




	}
}
