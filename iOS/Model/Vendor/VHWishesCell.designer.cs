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
    [Register ("VHWishesCell")]
    partial class VHWishesCell
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
        UIKit.UILabel lblExpireDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQuery { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblUsername { get; set; }

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

            if (lblExpireDate != null) {
                lblExpireDate.Dispose ();
                lblExpireDate = null;
            }

            if (lblQuery != null) {
                lblQuery.Dispose ();
                lblQuery = null;
            }

            if (lblUsername != null) {
                lblUsername.Dispose ();
                lblUsername = null;
            }

            if (viewBG != null) {
                viewBG.Dispose ();
                viewBG = null;
            }
        }
    }
}