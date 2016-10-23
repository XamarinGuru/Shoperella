using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class FavoritesViewController : BaseViewController
	{
		public FavoritesViewController(IntPtr handle) : base (handle)
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
				ShowLoadingView("Retriving User Data...");

				var favorites = client.GetUserFavorites(AppDelegate.User);

				HideLoadingView();

				var tblDataSource = new FavoritesTableViewSource(favorites, this);
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
				var favorites = client.GetUserFavorites(AppDelegate.User);

				var tblDataSource = new FavoritesTableViewSource(favorites, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}

		private async void CellCallBack(string dealID)
		{
			var client = new Client();

			ShowLoadingView("Processing...");

			var deals = await client.RemoveFromFavorite(dealID, AppDelegate.User);
			if (deals == null)
			{
				HideLoadingView();
				ShowMessageBox(null, "Failed...");
				return;
			}

			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var favorites = client.GetUserFavorites(AppDelegate.User);

				var tblDataSource = new FavoritesTableViewSource(favorites, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});

				HideLoadingView();
			});
		}
		#region FavoritesTableViewSource

		class FavoritesTableViewSource : UITableViewSource
		{
			private List<Deal> favorites;
			private FavoritesViewController offersVC;

			public FavoritesTableViewSource(List<Deal> favorites, FavoritesViewController offersVC)
			{
				this.favorites = new List<Deal>();

				if (favorites == null) return;

				this.favorites = favorites;
				this.offersVC = offersVC;
			}

			public override nint NumberOfSections(UITableView tableView)
			{
				return favorites.Count;
			}
			public override nfloat GetHeightForHeader(UITableView tableView, nint section)
			{
				return 50.0f;
			}
			public override UIView GetViewForHeader(UITableView tableView, nint section)
			{
				FavoriteSectionHeaderCell cell = tableView.DequeueReusableCell("SectionHeaderCell") as FavoriteSectionHeaderCell;
				cell.SetTitle(favorites[(int)section].Vendor.Name);

				return cell;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return 1;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				FavoriteCell cell = tableView.DequeueReusableCell("FavoriteCell") as FavoriteCell;
				cell.SetCell(favorites[indexPath.Section], offersVC.CellCallBack);

				return cell;
			}
		}
		#endregion
	}
}

