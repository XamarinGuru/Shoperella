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
    [Register ("DailyDealsViewController")]
    partial class DailyDealsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnGo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView map { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtSearchkey { get; set; }

        [Action ("OnClickSearchGo:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnClickSearchGo (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnGo != null) {
                btnGo.Dispose ();
                btnGo = null;
            }

            if (map != null) {
                map.Dispose ();
                map = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }

            if (txtSearchkey != null) {
                txtSearchkey.Dispose ();
                txtSearchkey = null;
            }
        }
    }
}