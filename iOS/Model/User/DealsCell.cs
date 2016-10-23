using System;

using Foundation;
using UIKit;

using SDWebImage;

using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class DealsCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("DailyDealsCell");
		public static readonly UINib Nib;

		private Action<string> callback;

		static DealsCell()
		{
			Nib = UINib.FromName("DailyDealsCell", NSBundle.MainBundle);
		}

		public DealsCell(IntPtr handle) : base (handle)
		{
		}

		public void SetCell(Deal deal, Action<string> callback)
		{
			this.callback = callback;

			if (deal.Vendor.ImagePath != null)
				img.SetImage(
					url: new NSUrl(deal.Vendor.ImagePath),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);

			lblTitle.Text = deal.Title;
			lblCaption.Text = deal.Caption;
			btnSave.Tag = int.Parse(deal.ID);
		}

		override public void LayoutSubviews()
		{
			base.LayoutSubviews();

			imgBorder.LayoutIfNeeded();
			imgBorder.Layer.CornerRadius = 4;
			imgBorder.Layer.MasksToBounds = true;

			img.LayoutIfNeeded();
			img.Layer.CornerRadius = 20;
			img.Layer.BorderWidth = 1;
			img.Layer.BorderColor = UIColor.Gray.CGColor;
			img.Layer.MasksToBounds = true;

			btnSave.LayoutIfNeeded();
			btnSave.Layer.CornerRadius = 3;
			btnSave.Layer.MasksToBounds = true;
		}

		partial void OnClickSave(UIButton sender)
		{
			var dealID = sender.Tag.ToString();
			callback(dealID);
		}
	}
}
