// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Shoperella.iOS
{
    [Register ("VHOpenOffersCell")]
    partial class VHOpenOffersCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBorder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDealCaption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDealName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblExpireDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblVendorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewBG { get; set; }

        [Action ("OnClickView:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickView (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnView != null) {
                btnView.Dispose ();
                btnView = null;
            }

            if (img != null) {
                img.Dispose ();
                img = null;
            }

            if (imgBorder != null) {
                imgBorder.Dispose ();
                imgBorder = null;
            }

            if (lblDealCaption != null) {
                lblDealCaption.Dispose ();
                lblDealCaption = null;
            }

            if (lblDealName != null) {
                lblDealName.Dispose ();
                lblDealName = null;
            }

            if (lblExpireDate != null) {
                lblExpireDate.Dispose ();
                lblExpireDate = null;
            }

            if (lblVendorName != null) {
                lblVendorName.Dispose ();
                lblVendorName = null;
            }

            if (viewBG != null) {
                viewBG.Dispose ();
                viewBG = null;
            }
        }
    }
}