using System;
using FlyoutNavigation;
using UIKit;
using MonoTouch.Dialog;
using CoreGraphics;
using Facebook.LoginKit;
using Facebook.CoreKit;
using Foundation;

namespace Shoperella.iOS
{
	public class Navigation : FlyoutNavigationController
	{
		public static Navigation Instance { get; set; }
		public UIStoryboard Storybard { get; set; }
		private enum MenuItemType { Regular, Profile, LoginLogout, Link }

		public Navigation(UIStoryboard Storyboard) : base()
		{
			this.Storybard = Storyboard;
			Instance = this;

			if (AppDelegate.CurrentContext == Settings.CONTEXT_USER)
				GenerateUserNav();
			else if (AppDelegate.CurrentContext == Settings.CONTEXT_VENDOR)
				GenerateVendorNav();

			NavigationTableView.BackgroundColor = Colors.Green;
			NavigationTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			var view = Foundation.NSBundle.MainBundle.LoadNib("NavHeader", this, null);
			var HeaderView = view.GetItem<UIView>(0) as NavHeaderView;

			AppDelegate.NavHeaderView = HeaderView;

			if (AppDelegate.CurrentContext == Settings.CONTEXT_USER)
			{
				HeaderView.SetName("Hey, " + AppDelegate.User.name);
				if (AppDelegate.User.photoURL != null)
					HeaderView.SetImage(AppDelegate.User.photoURL);
			}
			else if (AppDelegate.CurrentContext == Settings.CONTEXT_VENDOR)
			{
				HeaderView.SetName("Hey, " + AppDelegate.Vendor.Name);
				if (AppDelegate.Vendor.ImagePath != null)
					HeaderView.SetImage(AppDelegate.Vendor.ImagePath);
			}
			HeaderView.Bounds = new CoreGraphics.CGRect(0, 0, 160, 220);
			NavigationTableView.TableHeaderView = view.GetItem<UIView>(0);
			ToggleMenu();
		}

		private void GenerateUserNav()
		{
			// Create the navigation menu
			NavigationRoot = new RootElement("Navigation") {
				new Section () {
					GetStringElement ("HOME", MenuItemType.Regular),
					GetStringElement ("DAILY DEALS", MenuItemType.Regular),
					GetStringElement ("OFFERS", MenuItemType.Regular),
					GetStringElement ("FAVORITES", MenuItemType.Regular),
				},
			};

			if (AppDelegate.User == null)
			{
				NavigationRoot.Add(
					new Section(new UIView(new CGRect(0, 0, 10, 20))) {
						GetStringElement("Login", MenuItemType.Profile)
					}
				);
			}
			else {
				NavigationRoot.Add(
					new Section(new UIView(new CGRect(0, 0, 10, 20))) {
						GetStringElement("My Profile", MenuItemType.Profile)
					}
				);
			}

			NavigationRoot.Add(
				new Section(new UIView(new CGRect(0, 0, 10, 20))) {
					GetStringElement("Contact Us", MenuItemType.Link),
					GetStringElement("Terms & Conditions", MenuItemType.Link)
			});

			// Supply view controllers corresponding to menu items:
			ViewControllers = new[] {
				 new UINavigationController(Storybard.InstantiateViewController("Home")),
				 new UINavigationController(Storybard.InstantiateViewController("DailyDealsViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("OffersViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("FavoritesViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("UserProfileViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("Home")),
			};

			NavigationRoot.UnevenRows = true;

		}

		private void GenerateVendorNav()
		{
			// Create the navigation menu
			NavigationRoot = new RootElement("Navigation") {
				new Section () {
					GetStringElement ("HOME", MenuItemType.Regular),
					GetStringElement ("CREATE OFFER", MenuItemType.Regular),
					GetStringElement ("INBOX", MenuItemType.Regular),
					GetStringElement ("OPEN OFFERS", MenuItemType.Regular),
				},
				new Section(new UIView(new CGRect(0, 0, 10, 20))) {
					GetStringElement("My Profile", MenuItemType.Profile)
				},
				new Section(new UIView(new CGRect(0, 0, 10, 20))) {
					GetStringElement("Contact Us", MenuItemType.Link),
					GetStringElement("Terms & Conditions", MenuItemType.Link)
				}
			};

			// Supply view controllers corresponding to menu items:
			ViewControllers = new[] {
				 new UINavigationController(Storybard.InstantiateViewController("VendorHomeViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("InboxViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("InboxViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("OpenOffersViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("VendorProfileViewController")),
				 new UINavigationController(Storybard.InstantiateViewController("Home")),
			};
		}

		private StyledStringElement GetStringElement(string text, MenuItemType type)
		{
			var el = new StyledStringElement(text);
			el.BackgroundColor = UIColor.Clear;

			switch (type)
			{
				case MenuItemType.Regular:
					el.TextColor = UIColor.White;
					el.Font = UIFont.FromName("NexaLight", 20);
					break;
				case MenuItemType.Profile:
					el.TextColor = Colors.Pink;
					el.Font = UIFont.FromName("NexaBold", 18);
					break;
				case MenuItemType.LoginLogout:
					el.TextColor = UIColor.White;
					el.Font = UIFont.FromName("NexaLight", 18);
					break;
				case MenuItemType.Link:
					el = new ShortStyledStringElement(text);
					el.TextColor = UIColor.White;
					el.Font = UIFont.FromName("NexaLight", 12);

					break;
			}

			return el;
		}

		internal class ShortStyledStringElement : StyledStringElement, IElementSizing
		{
			public ShortStyledStringElement(string caption) : base(caption)
			{
			}

			public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
			{
				return 20;
			}
		}
	}
}

