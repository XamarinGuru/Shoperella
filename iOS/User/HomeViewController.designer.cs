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
    [Register ("HomeViewController")]
    partial class HomeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnGO { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton checkShowDialog { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgProfilePhoto { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtWish { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewAlert { get; set; }

        [Action ("OnClickCreateWish:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickCreateWish (UIKit.UIButton sender);

        [Action ("OnClickSearchDailyDeals:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickSearchDailyDeals (UIKit.UIButton sender);

        [Action ("OnClickShowAgain:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickShowAgain (UIKit.UIButton sender);

        [Action ("OnClickShowAlert:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickShowAlert (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnGO != null) {
                btnGO.Dispose ();
                btnGO = null;
            }

            if (btnMenu != null) {
                btnMenu.Dispose ();
                btnMenu = null;
            }

            if (btnSearch != null) {
                btnSearch.Dispose ();
                btnSearch = null;
            }

            if (checkShowDialog != null) {
                checkShowDialog.Dispose ();
                checkShowDialog = null;
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

            if (txtWish != null) {
                txtWish.Dispose ();
                txtWish = null;
            }

            if (viewAlert != null) {
                viewAlert.Dispose ();
                viewAlert = null;
            }
        }
    }
}