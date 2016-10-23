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
    [Register ("OfferDetailViewController")]
    partial class OfferDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFavorites { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCaption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTimeRemain { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView txtDescription { get; set; }

        [Action ("OnClickAddToFavorite:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickAddToFavorite (UIKit.UIButton sender);

        [Action ("OnClickNoThanks:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickNoThanks (UIKit.UIButton sender);

        [Action ("OnClickRedeem:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickRedeem (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnFavorites != null) {
                btnFavorites.Dispose ();
                btnFavorites = null;
            }

            if (img != null) {
                img.Dispose ();
                img = null;
            }

            if (lblCaption != null) {
                lblCaption.Dispose ();
                lblCaption = null;
            }

            if (lblTimeRemain != null) {
                lblTimeRemain.Dispose ();
                lblTimeRemain = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (txtDescription != null) {
                txtDescription.Dispose ();
                txtDescription = null;
            }
        }
    }
}