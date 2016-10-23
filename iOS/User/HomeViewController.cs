using Foundation;
using System;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;

using SDWebImage;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class HomeViewController : BaseViewController
    {
        public HomeViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var tap = new UITapGestureRecognizer(() => { View.EndEditing(true); });
			View.AddGestureRecognizer(tap);

			SetRImage(imgProfilePhoto);
			SetRButton(btnGO);

			btnMenu.TouchUpInside += delegate {
				Navigation.Instance.ToggleMenu();
			};

			AppendPullToRefresh(this, TableView, Refresh);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			HeaderStyleGenerator.Generate(this, showNavBar: false);

			if (AppDelegate.User != null)
			{
				imgProfilePhoto.SetImage(
					url: new NSUrl(AppDelegate.User.photoURL),
					placeholder: UIImage.FromBundle("icon_vendor.jpg")
				);
				lblName.Text = AppDelegate.User.name;
			}

			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var offers = client.GetUserOffers(AppDelegate.User);
				var favorites = client.GetUserFavorites(AppDelegate.User);

				var tblDataSource = new HomeTableViewSource(offers, favorites, this);
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
				var offers = client.GetUserOffers(AppDelegate.User);
				var favorites = client.GetUserFavorites(AppDelegate.User);

				var tblDataSource = new HomeTableViewSource(offers, favorites, this);
				this.InvokeOnMainThread(delegate
				{
					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}

		private void CreateWish()
		{
			var client = new Client();

			var wishQuery = txtWish.Text;
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView("Creating your wish...");

				var wishJson = client.CreateWish(wishQuery, AppDelegate.Latitude, AppDelegate.Longitude, AppDelegate.User);

				HideLoadingView();

				if (wishJson == null)
				{
					ShowMessageBox(null, "create failed");
				}
				else
				{
					this.InvokeOnMainThread(delegate
					{
						viewAlert.Hidden = true;
						txtWish.Text = "";
					});
					ShowMessageBox(null, "Vendors in the area have been notified of your request");
				}
			});
		}

		partial void OnClickShowAlert(UIButton sender)
		{
			if (txtWish.Text.Equals(string.Empty))
			{
				ShowMessageBox(null, "please specify wish query");
				return;
			}

			if (!Settings.IsCheckedShowWishAlertAgain)
			{
				viewAlert.Hidden = false;
				return;
			}
			CreateWish();
		}

		partial void OnClickShowAgain(UIButton sender)
		{
			bool isSelected = sender.Selected;
			sender.Selected = !isSelected;
			Settings.IsCheckedShowWishAlertAgain = !isSelected;
		}

		partial void OnClickCreateWish(UIButton sender)
		{
			CreateWish();
		}

		partial void OnClickSearchDailyDeals(UIButton sender)
		{
			var dailyDealsVC = Storyboard.InstantiateViewController("DailyDealsViewController") as DailyDealsViewController;
			dailyDealsVC.fromWhere = "home";
			NavigationController.PushViewController(dailyDealsVC, true);
		}

		#region HomeTableViewSource

		class HomeTableViewSource : UITableViewSource
		{
			private List<Offer> offers;
			private List<Deal> favorites;
			private HomeViewController homeVC;

			public HomeTableViewSource(List<Offer> offers, List<Deal> favorites, HomeViewController homeVC)
			{
				this.offers = new List<Offer>();
				this.favorites = new List<Deal>();

				if (offers != null)
					this.offers = offers;

				if (favorites != null)
					this.favorites = favorites;

				this.homeVC = homeVC;
			}

			public override nint NumberOfSections(UITableView tableView)
			{
				return 2;
			}
			public override nfloat GetHeightForHeader(UITableView tableView, nint section)
			{
				return 50.0f;
			}
			public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				return 60.0f;
			}

			public override UIView GetViewForHeader(UITableView tableView, nint section)
			{
				HomeSectionHeaderCell cell = tableView.DequeueReusableCell("HomeSectionHeaderCell") as HomeSectionHeaderCell;
				var headerTitle = section == 0 ? "OFFERS" : "FAVORITES";
				var count = section == 0 ? offers.Count : favorites.Count;
				cell.SetCell(headerTitle, count);

				return cell;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				if (section == 0)
					return offers.Count;
				else
					return favorites.Count;
				
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0)
				{
					HomeCell cell = tableView.DequeueReusableCell("HomeCell") as HomeCell;
					cell.SetCell(offers[indexPath.Row], homeVC);

					return cell;
				}
				else {
					HomeCell cell = tableView.DequeueReusableCell("HomeCell") as HomeCell;
					cell.SetCell(favorites[indexPath.Row], homeVC);

					return cell;
				}
			}


			//public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			//{
			//	var detailVC = offersVC.Storyboard.InstantiateViewController("DealDetailViewController") as DealDetailViewController;
			//	detailVC.Deal = deals[indexPath.Section];
			//	offersVC.NavigationController.PushViewController(detailVC, true);
			//}

		}
		#endregion
    }
}