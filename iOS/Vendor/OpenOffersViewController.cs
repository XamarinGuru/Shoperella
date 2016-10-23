using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class OpenOffersViewController : BaseViewController
	{
		public OpenOffersViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this);

			Refresh();
			AppendPullToRefresh(this, TableView, Refresh);

		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Retriving Open Offers...");

				var offers = client.GetVendorOffers(AppDelegate.Vendor, AppDelegate.User);

				HideLoadingView();

				var tblDataSource = new OpenOffersTableViewSource(offers, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}

		private void Refresh()
		{
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var offers = client.GetVendorOffers(AppDelegate.Vendor, AppDelegate.User);

				var tblDataSource = new OpenOffersTableViewSource(offers, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}

		private void RenewCallBack(string offerID)
		{
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Processing...");

				var offer = client.RenewOffer(offerID, AppDelegate.User);
				if (offer == null)
				{
					HideLoadingView();
					ShowMessageBox(null, "Failed renew offer");
					return;
				}

				HideLoadingView();
				ShowMessageBox(null, "Success renew offer");
			});
		}
		private void RemoveCallBack(string offerID)
		{
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Processing...");

				var offer = client.DeleteOffer(offerID, AppDelegate.User);
				if (offer == null)
				{
					HideLoadingView();
					ShowMessageBox(null, "Failed remove offer");
					return;
				}

				var offers = client.GetVendorOffers(AppDelegate.Vendor, AppDelegate.User);

				HideLoadingView();

				var tblDataSource = new OpenOffersTableViewSource(offers, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
			client.DeleteOffer(offerID, AppDelegate.User);
		}


		#region OpenOffersTableViewSource

		class OpenOffersTableViewSource : UITableViewSource
		{
			private List<Offer> offers;
			private OpenOffersViewController offersVC;

			public OpenOffersTableViewSource(List<Offer> offers, OpenOffersViewController offersVC)
			{
				this.offers = new List<Offer>();
				this.offers = offers;
				this.offersVC = offersVC;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return offers.Count;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				OpenOffersCell cell = tableView.DequeueReusableCell("OpenOffersCell") as OpenOffersCell;
				cell.SetCell(offers[indexPath.Row], offersVC.RenewCallBack, offersVC.RemoveCallBack);

				return cell;
			}
		}
		#endregion
	}
}

