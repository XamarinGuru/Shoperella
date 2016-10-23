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
    [Register ("VendorHomeViewController")]
    partial class VendorHomeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCreateDeal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSetAutoResponse { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgProfilePhoto { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TableView { get; set; }

        [Action ("OnClickCreateDeal:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickCreateDeal (UIKit.UIButton sender);

        [Action ("OnClickSetAutoResponse:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickSetAutoResponse (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCreateDeal != null) {
                btnCreateDeal.Dispose ();
                btnCreateDeal = null;
            }

            if (btnMenu != null) {
                btnMenu.Dispose ();
                btnMenu = null;
            }

            if (btnSetAutoResponse != null) {
                btnSetAutoResponse.Dispose ();
                btnSetAutoResponse = null;
            }

            if (btnTCount != null) {
                btnTCount.Dispose ();
                btnTCount = null;
            }

            if (imgProfilePhoto != null) {
                imgProfilePhoto.Dispose ();
                imgProfilePhoto = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}