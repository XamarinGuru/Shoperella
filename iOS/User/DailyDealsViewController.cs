using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class DailyDealsViewController : BaseViewController
	{
		public string fromWhere = "nav";

		public DailyDealsViewController(IntPtr handle) : base (handle)
        {
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var tap = new UITapGestureRecognizer(() => { View.EndEditing(true); });
			View.AddGestureRecognizer(tap);

			SetRButton(btnGo);

			Refresh();
			AppendPullToRefresh(this, TableView, Refresh);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			if (fromWhere == "home")
				HeaderStyleGenerator.Generate(this, showMenu: false);
			else
				HeaderStyleGenerator.Generate(this);
			
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Retriving Daily Deals...");

				var vendors = client.GetVendors(AppDelegate.Latitude, AppDelegate.Longitude);
				var dailyDeals = client.GetDeals(AppDelegate.User);

				var tblDataSource = new DealsTableViewSource(dailyDeals, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
					UpdateDeals(vendors);
				});

				HideLoadingView();
			});
		}

		private void Refresh()
		{
			txtSearchkey.Text = "";

			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var vendors = client.GetVendors(AppDelegate.Latitude, AppDelegate.Longitude);
				var dailyDeals = client.GetDeals(AppDelegate.User);

				var tblDataSource = new DealsTableViewSource(dailyDeals, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
					UpdateDeals(vendors);
				});
			});
		}

		public void UpdateDeals(List<Vendor> vendors)
		{
			map.SetRegion(new MapKit.MKCoordinateRegion(new CoreLocation.CLLocationCoordinate2D(AppDelegate.Latitude, AppDelegate.Longitude), new MapKit.MKCoordinateSpan(0.5, 0.5)), true);

			if (vendors == null) return;

			foreach (var vendor in vendors)
			{
				if (vendor.Deals == null) continue;

				foreach (var deal in vendor.Deals)
				{
					map.AddAnnotation(new MapKit.MKPointAnnotation()
					{
						Title = deal.Vendor.Name,
						Coordinate = new CoreLocation.CLLocationCoordinate2D(deal.Vendor.Latitude, deal.Vendor.Longitude)
					});
				}
			}
		}

		partial void OnClickSearchGo(UIButton sender)
		{
			if (txtSearchkey.Text.Equals(string.Empty))
			{
				ShowMessageBox(null, "please specify wish query");
				return;
			}

			var client = new Client();

			var searchkey = txtSearchkey.Text;
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Searching deals...");

				var dailyDeals = client.SearchDeals(searchkey, AppDelegate.User);

				var tblDataSource = new DealsTableViewSource(dailyDeals, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});

				HideLoadingView();
			});
		}

		private async void CellCallBack(string dealID)
		{
			var client = new Client();

			ShowLoadingView("Saving to favorite...");

			var deals = await client.AddToFavorite(dealID, AppDelegate.User);
			if (deals == null)
				ShowMessageBox(null, "Already saved to favorite...");

			HideLoadingView();
		}
		#region DealsTableViewDataSource

		class DealsTableViewSource : UITableViewSource
		{
			private List<Deal> deals;
			private DailyDealsViewController dealsVC;

			public DealsTableViewSource(List<Deal> deals, DailyDealsViewController dealsVC)
			{
				this.deals = new List<Deal>();

				if (deals == null) return;

				this.deals = deals;
				this.dealsVC = dealsVC;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				DealsCell cell = tableView.DequeueReusableCell("DailyDealsCell") as DealsCell;
				cell.SetCell(deals[indexPath.Section], dealsVC.CellCallBack);

				return cell;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return 1;
			}

			public override nint NumberOfSections(UITableView tableView)
			{
				return deals.Count;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				var detailVC = dealsVC.Storyboard.InstantiateViewController("DealDetailViewController") as DealDetailViewController;
				detailVC.deal = deals[indexPath.Section];
				dealsVC.NavigationController.PushViewController(detailVC, true);
			}
		}
		#endregion
	}
}

