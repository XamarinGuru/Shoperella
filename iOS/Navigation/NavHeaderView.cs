using Foundation;
using System;
using UIKit;

namespace Shoperella.iOS
{
    public partial class NavHeaderView : UIView
    {
		private string name;

        public NavHeaderView (IntPtr handle) : base (handle)
        {
        }

		public void SetName(string name)
		{
			this.name = name;
			lblName.Text = name;
		}

		public void SetImage(string imgPath)
		{
			img.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(imgPath)));
			img.Layer.CornerRadius = 71;
			img.Layer.BorderColor = UIColor.White.CGColor;
			img.Layer.BorderWidth = 2.0f;
			img.Layer.MasksToBounds = true;
		}
    }
}