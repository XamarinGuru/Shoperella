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
    [Register ("VendorProfileEditViewController")]
    partial class VendorProfileEditViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField lblAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField lblCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField lblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField lblST { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField lblZip { get; set; }

        [Action ("OnClickAutoResponse:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickAutoResponse (UIKit.UIButton sender);

        [Action ("OnClickChangeImage:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickChangeImage (UIKit.UIButton sender);

        [Action ("OnClickChangeLocation:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickChangeLocation (UIKit.UIButton sender);

        [Action ("OnClickSaveProfile:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickSaveProfile (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (img != null) {
                img.Dispose ();
                img = null;
            }

            if (lblAddress != null) {
                lblAddress.Dispose ();
                lblAddress = null;
            }

            if (lblCity != null) {
                lblCity.Dispose ();
                lblCity = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (lblST != null) {
                lblST.Dispose ();
                lblST = null;
            }

            if (lblZip != null) {
                lblZip.Dispose ();
                lblZip = null;
            }
        }
    }
}