using System;

using Foundation;
using UIKit;

using SDWebImage;

using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class WishesCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("WishesCell");
		public static readonly UINib Nib;

		private InboxViewController rootVC;

		static WishesCell()
		{
			Nib = UINib.FromName("WishesCell", NSBundle.MainBundle);
		}

		public WishesCell(IntPtr handle) : base(handle)
		{
		}

		public void SetCell(Wish wish, InboxViewController rootVC)
		{
			this.rootVC = rootVC;

			if (wish.User.photoURL != null)
				img.SetImage(
					url: new NSUrl(wish.User.photoURL),
					placeholder: UIImage.FromBundle("icon_avatar.png")
				);

			lblName.Text = wish.User.name;
			lblQuery.Text = wish.Query;
			btnSendOffer.Tag = int.Parse(wish.ID);
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

			btnSendOffer.LayoutIfNeeded();
			btnSendOffer.Layer.CornerRadius = 3;
			btnSendOffer.Layer.MasksToBounds = true;
		}

		partial void OnClickSendOffer(UIButton sender)
		{
			var createTemplateVC = rootVC.Storyboard.InstantiateViewController("CreateTemplateViewController") as CreateTemplateViewController;
			createTemplateVC.wishID = sender.Tag.ToString();
			createTemplateVC.templateType = "offer";
			rootVC.NavigationController.PushViewController(createTemplateVC, true);
		}
	}
}
