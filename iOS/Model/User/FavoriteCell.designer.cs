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
    [Register ("FavoriteCell")]
    partial class FavoriteCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnX { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBorder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCaption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Action ("OnClickX:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickX (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnX != null) {
                btnX.Dispose ();
                btnX = null;
            }

            if (img != null) {
                img.Dispose ();
                img = null;
            }

            if (imgBorder != null) {
                imgBorder.Dispose ();
                imgBorder = null;
            }

            if (lblCaption != null) {
                lblCaption.Dispose ();
                lblCaption = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }
        }
    }
}