using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class OffersViewController : BaseViewController
	{
		public OffersViewController(IntPtr handle) : base (handle)
        {
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			HeaderStyleGenerator.Generate(this);

			AppendPullToRefresh(this, TableView, Refresh);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			GetUserOffers();
		}

		private void GetUserOffers()
		{
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Processing...");

				var offers = client.GetUserOffers(AppDelegate.User);

				HideLoadingView();

				var tblDataSource = new UserOffersTableViewSource(offers, this);
				this.InvokeOnMainThread(delegate
				{
					lblCountOffers.Text = "You have " + offers.Count.ToString() + " Offers!";

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
				var offers = client.GetUserOffers(AppDelegate.User);

				var tblDataSource = new UserOffersTableViewSource(offers, this);
				this.InvokeOnMainThread(delegate
				{
					lblCountOffers.Text = "You have " + offers.Count.ToString() + " Offers!";

					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}

		private void CellCallBack(string offerID)
		{
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Processing...");

				var offer = client.DismissUserOffer(offerID, AppDelegate.User);
				if (offer == null)
				{
					HideLoadingView();
					ShowMessageBox(null, "Failed...");
					return;
				}

				var offers = client.GetUserOffers(AppDelegate.User);

				HideLoadingView();

				var tblDataSource = new UserOffersTableViewSource(offers, this);
				this.InvokeOnMainThread(delegate
				{
					lblCountOffers.Text = "You have " + offers.Count.ToString() + " Offers!";

					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}
		#region UserOffersTableViewSource

		class UserOffersTableViewSource : UITableViewSource
		{
			private List<Offer> offers;
			private OffersViewController offersVC;

			public UserOffersTableViewSource(List<Offer> offers, OffersViewController offersVC)
			{
				this.offers = new List<Offer>();

				if (offers == null) return;

				this.offers = offers;
				this.offersVC = offersVC;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return offers.Count;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				OffersCell cell = tableView.DequeueReusableCell("OffersCell") as OffersCell;
				cell.SetCell(offers[indexPath.Row], offersVC.CellCallBack, offersVC);

				return cell;
			}

		}
		#endregion
	}
}

