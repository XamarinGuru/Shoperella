using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class InboxViewController : BaseViewController
	{
		public InboxViewController(IntPtr handle) : base(handle)
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

			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Retriving User Requests...");

				var wishes = client.GetWishes(AppDelegate.Vendor.ID);

				var tblDataSource = new WishesTableViewSource(wishes, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});

				HideLoadingView();
			});
		}

		private void Refresh()
		{
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var wishes = client.GetWishes(AppDelegate.Vendor.ID);

				var tblDataSource = new WishesTableViewSource(wishes, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}

		#region FavoritesTableViewSource

		class WishesTableViewSource : UITableViewSource
		{
			private List<Wish> wishes;
			private InboxViewController inboxVC;

			public WishesTableViewSource(List<Wish> wishes, InboxViewController inboxVC)
			{
				this.wishes = wishes;
				this.inboxVC = inboxVC;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return wishes.Count;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				WishesCell cell = tableView.DequeueReusableCell("WishesCell") as WishesCell;
				cell.SetCell(wishes[indexPath.Row], inboxVC);

				return cell;
			}
		}
		#endregion
	}
}

