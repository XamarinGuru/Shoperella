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
    [Register ("VendorProfileViewController")]
    partial class VendorProfileViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEmail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblName { get; set; }

        [Action ("GoToEditProfile:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void GoToEditProfile (UIKit.UIButton sender);

        [Action ("OnClickSwitchToUser:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickSwitchToUser (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (img != null) {
                img.Dispose ();
                img = null;
            }

            if (lblEmail != null) {
                lblEmail.Dispose ();
                lblEmail = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }
        }
    }
}