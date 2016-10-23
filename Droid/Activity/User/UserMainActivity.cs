using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Net;

using Android.Support.V4.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace Shoperella.Droid
{
	[Activity(Label = "UserMainActivity")]
	public class UserMainActivity : BaseActivity
	{
		ListView mDrawerList;
		public RelativeLayout mDrawerPane;
		public DrawerLayout mDrawerLayout;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Window.RequestFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.UserMainLayout);

			initialSettings();
		}

		private void initialSettings()
		{
			//Load Home fragment
			Fragment fragment = new UserHomeFragment();
			FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
			fragmentTx.Replace(Resource.Id.mainFragmentContent, fragment).Commit();

			//Load Slide menu
			var avatar = (ImageView)FindViewById(Resource.Id.avatar);
			if (Settings.User.photoURL != null && Settings.User.photoURL != "")
			{
				Koush.UrlImageViewHelper.SetUrlDrawable(avatar, Settings.User.photoURL);
			}

			TextView username = (TextView)FindViewById(Resource.Id.menu_userName);
			username.Text = "Hey, " + Settings.User.name;

			mDrawerLayout = (DrawerLayout)FindViewById(Resource.Id.drawerLayout);
			mDrawerPane = (RelativeLayout)FindViewById(Resource.Id.drawerPane);
			mDrawerList = (ListView)FindViewById(Resource.Id.menuList);

			string[] navList = { "HOME", "DAILY DEALS", "OFFERS", "FAVORITES", "My Profile", "Contact Us", "Terms & Conditions" };

			MenuListAdapter adapter = new MenuListAdapter(this, navList);
			mDrawerList.Adapter = adapter;

			// Drawer Item click listeners
			mDrawerList.ItemClick += (sender, args) => ListItemClicked(args.Position);

			mDrawerLayout.OpenDrawer(mDrawerPane);
		}
		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}

		private void ListItemClicked(int position)
		{
			Fragment fragment = null;

			switch (position)
			{
				case 0:
					fragment = new UserHomeFragment();
					break;
				case 1:
					fragment = new UserDailyDealsFragment();
					break;
				case 2:
					fragment = new UserOffersFragment();
					break;
				case 3:
					fragment = new UserFavoritesFragment();
					break;
				case 4:
					fragment = new UserProfileFragment();
					break;
				default:
					return;
			}

			FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
			fragmentTx.Replace(Resource.Id.mainFragmentContent, fragment).Commit();

			mDrawerList.SetItemChecked(position, true);
			mDrawerLayout.CloseDrawer(mDrawerPane);
		}

	}
}
