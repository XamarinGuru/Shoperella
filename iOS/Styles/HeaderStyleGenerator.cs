using System;
using CoreGraphics;
using UIKit;

namespace Shoperella.iOS
{
	public class HeaderStyleGenerator
	{
		/// <summary>
		/// Applies the shoperella styles to the supplied navigation controller.  This is a white background with the logo centered and a menu button.
		/// </summary>
		/// <param name="NavigationController">Navigation controller.</param>
		public static void Generate(UIViewController ViewController, bool showLogo = true, bool showMenu = true, bool showNavBar = true)
		{
			ViewController.NavigationController.NavigationBarHidden = !showNavBar;
			ViewController.NavigationController.NavigationBar.BarTintColor = UIColor.White;
			ViewController.NavigationController.NavigationBar.ShadowImage = new UIImage();
			if (showMenu)
			{
				ViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("btn-menu.png"), UIBarButtonItemStyle.Plain, delegate { Navigation.Instance.ToggleMenu(); });
				ViewController.NavigationItem.LeftBarButtonItem.TintColor = Colors.Green;
				ViewController.NavigationItem.LeftBarButtonItem.ImageInsets = new UIEdgeInsets(5, 8, 5, 8);
			}
			if (showLogo)
				ViewController.NavigationItem.TitleView = GetTitleView();
			ViewController.NavigationController.NavigationBar.TintColor = Colors.Green;
		}

		public static UIView GetTitleView()
		{
			var titleView = new UIView(new CGRect(0, 0, 100, 40));
			var logoView = new UIImageView(UIImage.FromFile("header_logo.png"));
			logoView.Frame = new CGRect(0, 0, 100, 40);
			logoView.ContentMode = UIViewContentMode.ScaleAspectFit;
			titleView.AddSubview(logoView);

			return titleView;
		}
	}
}

