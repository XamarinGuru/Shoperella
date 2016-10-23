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
    [Register ("WishesCell")]
    partial class WishesCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSendOffer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBorder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQuery { get; set; }

        [Action ("OnClickSendOffer:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickSendOffer (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnSendOffer != null) {
                btnSendOffer.Dispose ();
                btnSendOffer = null;
            }

            if (img != null) {
                img.Dispose ();
                img = null;
            }

            if (imgBorder != null) {
                imgBorder.Dispose ();
                imgBorder = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (lblQuery != null) {
                lblQuery.Dispose ();
                lblQuery = null;
            }
        }
    }
}