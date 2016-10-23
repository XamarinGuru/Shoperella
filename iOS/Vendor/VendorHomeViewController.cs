using Foundation;
using System;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class VendorHomeViewController : BaseViewController
	{
		public VendorHomeViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetRImage(imgProfilePhoto);
			SetRButton(btnCreateDeal);
			SetRButton(btnSetAutoResponse);

			btnMenu.TouchUpInside += delegate
			{
				Navigation.Instance.ToggleMenu();
			};

			Refresh();
			AppendPullToRefresh(this, TableView, Refresh);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			HeaderStyleGenerator.Generate(this, showNavBar: false);

			if (AppDelegate.Vendor != null)
			{
				if (AppDelegate.Vendor.ImagePath != null)
					imgProfilePhoto.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(AppDelegate.Vendor.ImagePath)));

				lblName.Text = AppDelegate.Vendor.Name;
			}
		}

		private void Refresh()
		{
			var client = new Client();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var wishes = client.GetWishes(AppDelegate.Vendor.ID);
				var offers = client.GetVendorOffers(AppDelegate.Vendor, AppDelegate.User);

				var tblDataSource = new HomeTableViewSource(wishes, offers, this);
				this.InvokeOnMainThread(delegate
				{
					var tCount = wishes.Count + offers.Count;
					btnTCount.SetTitle(tCount.ToString(), UIControlState.Normal);

					TableView.Source = tblDataSource;
					TableView.ReloadData();
				});
			});
		}

		partial void OnClickSetAutoResponse(UIButton sender)
		{
			var createTemplateVC = Storyboard.InstantiateViewController("CreateTemplateViewController") as CreateTemplateViewController;
			createTemplateVC.templateType = "autoresponse";
			NavigationController.PushViewController(createTemplateVC, true);
		}

		partial void OnClickCreateDeal(UIButton sender)
		{
			var createTemplateVC = Storyboard.InstantiateViewController("CreateTemplateViewController") as CreateTemplateViewController;
			createTemplateVC.templateType = "deal";
			this.NavigationController.PushViewController(createTemplateVC, true);
		}




		#region HomeTableViewSource

		class HomeTableViewSource : UITableViewSource
		{
			private List<Wish> wishes;
			private List<Offer> offers;
			private VendorHomeViewController homeVC;

			public HomeTableViewSource(List<Wish> wishes, List<Offer> offers, VendorHomeViewController homeVC)
			{
				this.homeVC = homeVC;

				this.wishes = new List<Wish>();
				this.offers = new List<Offer>();

				this.wishes = wishes;
				this.offers = offers;
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
				var headerTitle = section == 0 ? "USER REQUESTS" : "OPEN OFFERS";
				var count = section == 0 ? wishes.Count : offers.Count;
				cell.SetCell(headerTitle, count);

				return cell;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				if (section == 0)
					return wishes.Count;
				else
					return offers.Count;

			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0)
				{
					VHWishesCell cell = tableView.DequeueReusableCell("VHWishesCell") as VHWishesCell;
					cell.SetCell(wishes[indexPath.Row], homeVC);

					return cell;
				}
				else {
					VHOpenOffersCell cell = tableView.DequeueReusableCell("VHOpenOffersCell") as VHOpenOffersCell;
					cell.SetCell(offers[indexPath.Row], homeVC);

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