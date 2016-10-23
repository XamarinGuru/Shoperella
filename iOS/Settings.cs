using Foundation;
using Newtonsoft.Json.Linq;

using Shoperella.Model;

namespace Shoperella.iOS
{
	public static class Settings
	{
		public const string CONTEXT_USER = "user";
		public const string CONTEXT_VENDOR = "vendor";

		public static string FacebookToken
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.StringForKey("shopFacebookToken");
			}
			set
			{
				NSUserDefaults.StandardUserDefaults.SetString(value, "shopFacebookToken");
			}
		}

		public static string FacebookUID
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.StringForKey("shopFacebookUID");
			}
			set
			{
				NSUserDefaults.StandardUserDefaults.SetString(value, "shopFacebookUID");
			}
		}

		public static string PushDeviceToken
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
			}
			set
			{
				NSUserDefaults.StandardUserDefaults.SetString(value, "PushDeviceToken");
			}
		}

		public static bool IsCheckedShowWishAlertAgain
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.BoolForKey("shopIsCheckedShowWishAlertAgain1");
			}
			set
			{
				NSUserDefaults.StandardUserDefaults.SetBool(value, "shopIsCheckedShowWishAlertAgain1");
			}
		}

		public static User User
		{
			get
			{
				var userJSON = NSUserDefaults.StandardUserDefaults.StringForKey("shopUserJson");
				if (userJSON != null)
					return new User(JObject.Parse(userJSON));
				else
					return null;
			}
		}

		public static void SetUserJSON(string userJson)
		{
			if (userJson == null)
			{
				NSUserDefaults.StandardUserDefaults.RemoveObject("shopUserJson");
			}
			else {
				NSUserDefaults.StandardUserDefaults.SetString(userJson, "shopUserJson");
			}
		}

		public static Vendor Vendor
		{
			get
			{
				var vendorJSON = NSUserDefaults.StandardUserDefaults.StringForKey("shopVendorJson");
				if (vendorJSON != null)
					return new Vendor(JObject.Parse(vendorJSON));
				else
					return null;
			}
		}

		public static void SetVendorJSON(string vendorJson)
		{
			if (vendorJson == null)
			{
				NSUserDefaults.StandardUserDefaults.RemoveObject("shopVendorJson");
			}
			else {
				NSUserDefaults.StandardUserDefaults.SetString(vendorJson, "shopVendorJson");
			}
		}

		public static double Latitude
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.DoubleForKey("shopLat");
			}
			set
			{
				NSUserDefaults.StandardUserDefaults.SetDouble(value, "shopLat");
			}
		}

		public static double Longitude
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.DoubleForKey("shopLong");
			}
			set
			{
				NSUserDefaults.StandardUserDefaults.SetDouble(value, "shopLong");
			}
		}

		public static string CurrentContext
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.StringForKey("shopContext");
			}
			set
			{
				NSUserDefaults.StandardUserDefaults.SetString(value, "shopContext");
			}
		}

	}
}

