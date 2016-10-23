using System;

using Foundation;
using UIKit;

namespace Shoperella.iOS
{
	public partial class HomeSectionHeaderCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("HomeSectionHeaderCell");
		public static readonly UINib Nib;

		static HomeSectionHeaderCell()
		{
			Nib = UINib.FromName("HomeSectionHeaderCell", NSBundle.MainBundle);
		}

		protected HomeSectionHeaderCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void SetCell(string title, int count)
		{
			lblTitle.Text = title;
			btnCount.SetTitle(count.ToString(), UIControlState.Normal);
		}

	}
}
