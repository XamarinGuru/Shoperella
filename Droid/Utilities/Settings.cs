using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Newtonsoft.Json.Linq;

using Shoperella.Model;

namespace Shoperella.Droid
{
	public static class Settings
	{
		public static BaseActivity currentActivity;

		private static ISharedPreferences _appSettings = Application.Context.GetSharedPreferences("App_settings", FileCreationMode.Private);

		private const string facebookTokenKey = "shopFacebookToken";
		public static string FacebookToken
		{
			get
			{
				return _appSettings.GetString(facebookTokenKey, null);
			}
			set
			{
				ISharedPreferencesEditor editor = _appSettings.Edit();
				editor.PutString(facebookTokenKey, value);
				editor.Apply();
			}
		}

		private const string facebookUIDKey = "shopFacebookUID";
		public static string FacebookUID
		{
			get
			{
				return _appSettings.GetString(facebookUIDKey, null);
			}
			set
			{
				ISharedPreferencesEditor editor = _appSettings.Edit();
				editor.PutString(facebookUIDKey, value);
				editor.Apply();
			}
		}

		private const string pushDeviceTokenKey = "pushDeviceToken";
		public static string PushDeviceToken
		{
			get
			{
				return _appSettings.GetString(pushDeviceTokenKey, null);
			}
			set
			{
				ISharedPreferencesEditor editor = _appSettings.Edit();
				editor.PutString(pushDeviceTokenKey, value);
				editor.Apply();
			}
		}

		private const string isCheckedShowWishAlertAgainKey = "shopIsCheckedShowWishAlertAgain1";
		public static bool IsCheckedShowWishAlertAgain
		{
			get
			{
				return _appSettings.GetBoolean(isCheckedShowWishAlertAgainKey, false);
			}
			set
			{
				ISharedPreferencesEditor editor = _appSettings.Edit();
				editor.PutBoolean(isCheckedShowWishAlertAgainKey, value);
				editor.Apply();
			}
		}

		private const string latitudeKey = "shopLat";
		public static long Latitude
		{
			get
			{
				return _appSettings.GetLong(latitudeKey, 0);
			}
			set
			{
				ISharedPreferencesEditor editor = _appSettings.Edit();
				editor.PutLong(latitudeKey, value);
				editor.Apply();
			}
		}

		private const string shopLongKey = "shopLong";
		public static long Longitude
		{
			get
			{
				return _appSettings.GetLong(shopLongKey, 0);
			}
			set
			{
				ISharedPreferencesEditor editor = _appSettings.Edit();
				editor.PutLong(shopLongKey, value);
				editor.Apply();
			}
		}

		private const string currentContextKey = "shopContext";
		public static string CurrentContext
		{
			get
			{
				return _appSettings.GetString(currentContextKey, null);
			}
			set
			{
				ISharedPreferencesEditor editor = _appSettings.Edit();
				editor.PutString(currentContextKey, value);
				editor.Apply();
			}
		}



		private const string userKey = "shopUserJson";
		public static User User
		{
			get
			{
				var userJSON = _appSettings.GetString(userKey, null);
				if (userJSON != null)
					return new User(JObject.Parse(userJSON));
				else
					return null;
			}
		}
		public static void SetUserJSON(string userJson)
		{
			ISharedPreferencesEditor editor = _appSettings.Edit();

			if (userJson == null)
			{
				editor.Remove(userKey);
			}
			else {
				editor.PutString(userKey, userJson);
			}
			editor.Apply();
		}

		private const string vendorKey = "shopVendorJson";
		public static Vendor Vendor
		{
			get
			{
				var vendorJSON = _appSettings.GetString(vendorKey, null);
				if (vendorJSON != null)
					return new Vendor(JObject.Parse(vendorJSON));
				else
					return null;
			}
		}
		public static void SetVendorJSON(string vendorJson)
		{
			ISharedPreferencesEditor editor = _appSettings.Edit();

			if (vendorJson == null)
			{
				editor.Remove(vendorKey);
			}
			else {
				editor.PutString(vendorKey, vendorJson);
			}
			editor.Apply();
		}
	}
}

