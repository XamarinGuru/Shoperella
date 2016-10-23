using System;

using Foundation;
using UIKit;

namespace Shoperella.iOS
{
	public partial class FavoriteSectionHeaderCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("SectionHeaderCell");
		public static readonly UINib Nib;

		static FavoriteSectionHeaderCell()
		{
			Nib = UINib.FromName("SectionHeaderCell", NSBundle.MainBundle);
		}

		public FavoriteSectionHeaderCell(IntPtr handle) : base(handle)
		{
		}

		public void SetTitle(string title)
		{
			lblTitle.Text = title;
		}

	}
}
