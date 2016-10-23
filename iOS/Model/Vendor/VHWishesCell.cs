using System;

using Foundation;
using UIKit;

using SDWebImage;

using Shoperella.Model;
using Shoperella.API;

namespace Shoperella.iOS
{
	public partial class VHWishesCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("VHWishesCell");
		public static readonly UINib Nib;

		private VendorHomeViewController rootVC;

		static VHWishesCell()
		{
			Nib = UINib.FromName("VHWishesCell", NSBundle.MainBundle);
		}

		public VHWishesCell(IntPtr handle) : base(handle)
		{
		}

		public void SetCell(Wish wish, VendorHomeViewController rootVC)
		{
			this.rootVC = rootVC;

			if (wish.User.photoURL != null)
				img.SetImage(
					url: new NSUrl(wish.User.photoURL),
					placeholder: UIImage.FromBundle("icon_avatar.png")
				);
				//img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(wish.User.photoURL)));

			lblUsername.Text = wish.User.name;
			//lblExpireDate.Text = wish.;
			lblQuery.Text = "Searched for \"" + wish.Query + "\"";
			btnView.Tag = int.Parse(wish.ID);
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
