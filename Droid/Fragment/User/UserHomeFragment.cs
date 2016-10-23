using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.Net;

using System;

using Fragment = Android.Support.V4.App.Fragment;

namespace Shoperella.Droid
{
	public class UserHomeFragment : Fragment
	{
		UserMainActivity mActivity;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.UserHomeLayout, container, false);

			mActivity = this.Activity as UserMainActivity;

			var avatar = (ImageView)view.FindViewById(Resource.Id.avatar);
			if (Settings.User.photoURL != null && Settings.User.photoURL != "")
			{
				//var imageBitmap = GetImageBitmapFromUrl(Settings.User.photoURL);
				//avatar.SetImageBitmap(imageBitmap);
				Koush.UrlImageViewHelper.SetUrlDrawable(avatar, Settings.User.photoURL);
			}

			TextView username = (TextView)view.FindViewById(Resource.Id.userName);
			username.Text = Settings.User.name;

			ImageButton menuIconImageView = (ImageButton)view.FindViewById(Resource.Id.menuIconImgView);
			menuIconImageView.Click += delegate (object sender, EventArgs e)
			{
				mActivity.mDrawerLayout.OpenDrawer(mActivity.mDrawerPane);
			};

			return view;
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
	}
}
