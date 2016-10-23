using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using UIKit;
using BigTed;
using Foundation;

using Newtonsoft.Json.Linq;

using Shoperella.API;
using Shoperella.Model;

namespace Shoperella.iOS
{
	public partial class BaseViewController : UIViewController
	{
		private UIRefreshControl mRefreshControl;

		public BaseViewController(IntPtr handle) : base (handle)
        {
		}

		protected async Task SetUserDetail(User user)
		{
			var apiClient = new Client();

			#if (!DEBUG)
			var deviceToken = Settings.PushDeviceToken;
			var response = await apiClient.SendPushDeviceToken(deviceToken, user);
			#endif

			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				var userJson = apiClient.GetUserDetail(AppDelegate.User);
				var userData = JObject.Parse(userJson);

				if (userData["vendors"].Count() == 0)
					return;

				AppDelegate.Vendor = new Vendor(JObject.Parse(userData["vendors"][0].ToString()));
				Settings.SetVendorJSON(userData["vendors"][0].ToString());
			});
		}

		protected void AppendPullToRefresh(UIViewController targetVC, UITableView targetTV, Action callback)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				// the refresh control is available, let's add it
				mRefreshControl = new UIRefreshControl();
				mRefreshControl.ValueChanged += (sender, e) =>
				{
					mRefreshControl.BeginRefreshing();

					callback();

					mRefreshControl.EndRefreshing();
				};

			}

			targetTV.Add(mRefreshControl);
		}

		protected void ShowLoadingView(string title)
		{
			InvokeOnMainThread(() => { BTProgressHUD.Show(title, -1, ProgressHUD.MaskType.Black); });
		}

		protected void HideLoadingView()
		{
			InvokeOnMainThread(() => { BTProgressHUD.Dismiss(); });
		}
		// Show the alert view
		protected void ShowMessageBox(string title, string message, string cancelButton, string[] otherButtons, Action successHandler)
		{
			InvokeOnMainThread(delegate
			{
				var alertView = new UIAlertView(title, message, null, cancelButton, otherButtons);
				alertView.Clicked += (sender, e) =>
				{
					if (e.ButtonIndex == 0)
					{
						return;
					}
					if (successHandler != null)
					{
						successHandler();
					}
				};
				alertView.Show();
			});
		}

		//overloaded method
		protected void ShowMessageBox(string title, string message)
		{
			ShowMessageBox(title, message, "Ok", null, null);
		}

		protected bool TextFieldShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}

		//customize UI
		protected void SetRButton(UIButton button)
		{
			button.Layer.CornerRadius = 5;
		}

		protected void SetRImage(UIImageView image)
		{
			image.LayoutIfNeeded();
			image.Layer.CornerRadius = image.Frame.Size.Width / 2;
			image.Layer.BorderColor = UIColor.White.CGColor;
			image.Layer.BorderWidth = 2.0f;
			image.Layer.MasksToBounds = true;
		}

		//protected void SetRCellBorder(UIImageView image)
		//{
		//	image.LayoutIfNeeded();
		//	image.Layer.CornerRadius = 4;
		//	image.Layer.BorderColor = UIColor.Gray.CGColor;
		//	image.Layer.BorderWidth = 2.0f;
		//	image.Layer.MasksToBounds = true;
		//}

		/// <summary>
		/// Setups the time picker.
		/// </summary>
		/// <param name="field">Field.</param>
		/// <param name="mode">Mode.</param>
		/// <param name="format">Format. Like "{0: hh:mm tt}" or "{0: MM/dd/yyyy hh:mm tt}" </param>
		/// <param name="changeOnEdit">If set to <c>true</c> change on edit.</param>
		protected void SetupTimePicker(UITextField field, UIDatePickerMode mode = UIDatePickerMode.Time, String format = "{0: hh:mm tt}", bool futureDatesOnly = false, DateTime? minimumDateTime = null, bool changeOnEdit = false)
		{
			UIDatePicker picker = new UIDatePicker();
			picker.Mode = mode;

			if (minimumDateTime != null)
			{
				NSDate nsMinDateTime = ToNSDate((DateTime)minimumDateTime);
				picker.MinimumDate = nsMinDateTime;
			}
			if (futureDatesOnly)
			{
				NSDate nsMinDateTime = ToNSDate(DateTime.Now);
				picker.MinimumDate = nsMinDateTime;
			}

			picker.ValueChanged += (object s, EventArgs e) =>
			{
				if (futureDatesOnly)
				{
					NSDate nsMinDateTime = ToNSDate(DateTime.Now);
					picker.MinimumDate = nsMinDateTime;
				}
				if (changeOnEdit)
				{
					updateSetupDateTimePicker(field, picker.Date, format, s, e);
				}
			};

			// Setup the toolbar
			UIToolbar toolbar = new UIToolbar();
			toolbar.BarStyle = UIBarStyle.Black;
			toolbar.Translucent = true;
			toolbar.SizeToFit();

			// Create a 'done' button for the toolbar and add it to the toolbar
			UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (s, e) =>
			{
				updateSetupDateTimePicker(field, picker.Date, format, s, e, true);
			});

			toolbar.SetItems(new UIBarButtonItem[] { doneButton }, true);

			field.InputView = picker;
			field.InputAccessoryView = toolbar;

			field.ShouldChangeCharacters = new UITextFieldChange(delegate (UITextField textField, NSRange range, string replacementString)
			{
				return false;
			});
		}
		private void updateSetupDateTimePicker(UITextField field, NSDate date, String format, object sender, EventArgs e, bool done = false)
		{
			var newDate = NSDateToDateTime(date);
			var str = String.Format(format, newDate);

			field.Text = str;
			field.SendActionForControlEvents(UIControlEvent.ValueChanged);
			if (done)
			{
				field.ResignFirstResponder();
			}
		}
		private DateTime NSDateToDateTime(NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
			reference = reference.AddSeconds(date.SecondsSinceReferenceDate);
			if (reference.IsDaylightSavingTime())
			{
				reference = reference.AddHours(1);
			}
			return reference;
		}
		private NSDate ToNSDate(DateTime date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate(
				(date - reference).TotalSeconds);
		}

		protected string ConvertISODate(string strLocalDate)
		{
			NSDateFormatter ISOFormater = new NSDateFormatter();
			ISOFormater.DateFormat = "yyyy-MM-dd'T'HH:mm:ssZ";
			var pstr = DateTime.Parse(strLocalDate);
			var strReturnDate = ISOFormater.ToString(ToNSDate(pstr));
			return strReturnDate;
		}


		public UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
		{
			try
			{
				var sourceSize = sourceImage.Size;
				var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
				if (maxResizeFactor > 1) return sourceImage;
				var width = maxResizeFactor * sourceSize.Width;
				var height = maxResizeFactor * sourceSize.Height;
				UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
				sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
				var resultImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
				return resultImage;
			}
			catch
			{
				return null;
			}
		}

		public Dictionary<String, String> GetFormatedAddressInfo(PlaceDetailsAPI_RootObject locationInfo)
		{
			var zipData = locationInfo.result.address_components.SingleOrDefault(r => r.types.Contains("postal_code"));
			var zip = zipData.short_name.Replace(" ", "").Replace("-", "").Substring(0, 5);

			var streetData = locationInfo.result.address_components.SingleOrDefault(r => r.types.Contains("route"));
			var street = streetData == null ? string.Empty : streetData.long_name;

			var cityData = locationInfo.result.address_components.SingleOrDefault(r => r.types.Contains("locality"));
			var city = cityData == null ? string.Empty : cityData.long_name;

			var returnData = new Dictionary<String, String>
			{
				{"city", city },
				{"street", street},
				{"zip", zip}
			};

			return returnData;
		}
	}
}

