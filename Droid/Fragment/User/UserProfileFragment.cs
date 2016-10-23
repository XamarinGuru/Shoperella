using Android.Views;
using Android.Widget;
using Android.OS;

using System;

using Fragment = Android.Support.V4.App.Fragment;

namespace Shoperella.Droid
{
	public class UserProfileFragment : Fragment
	{
		UserMainActivity mActivity;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.UserProfileLayout, container, false);

			mActivity = this.Activity as UserMainActivity;

			ImageButton menuIconImageView = (ImageButton)view.FindViewById(Resource.Id.menuIconImgView);
			menuIconImageView.Click += delegate (object sender, EventArgs e)
			{
				mActivity.mDrawerLayout.OpenDrawer(mActivity.mDrawerPane);
			};

			return view;
		}
	}
}
