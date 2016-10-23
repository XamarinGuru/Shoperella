using Foundation;
using UIKit;
using System;
using System.Net.Http;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BigTed;
using CoreGraphics;
using Newtonsoft.Json;

namespace Shoperella.iOS
{
	public partial class LocationPickUpViewController : BaseTableViewController
	{
		public Action<PlaceDetailsAPI_RootObject> traDelegate;
		const string GOOGLE_PLACE_APIKEY = "AIzaSyAiBwRUm_KZDv_sp3eI7F8hxkePqDTvY20";
		ResultsTableController resultsTableController;
		UISearchController searchController;
		bool searchControllerWasActive;
		bool searchControllerSearchFieldWasFirstResponder;

		private UINavigationController thisController { get; set; }

		public LocationPickUpViewController() : base()
		{

		}

		public LocationPickUpViewController(IntPtr handle) : base(handle)
		{
			Title = "SELECT LOCATION";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			thisController = NavigationController;
			//NavigationItem.Customize(NavigationController);

			resultsTableController = new ResultsTableController
			{
				FilteredPredictions = new List<PlaceAutocompleteAPI_Prediction>()
			};

			searchController = new UISearchController(resultsTableController)
			{
				WeakDelegate = this,
				DimsBackgroundDuringPresentation = false,
				WeakSearchResultsUpdater = this
			};

			searchController.SearchBar.SizeToFit();

			TableView.TableHeaderView = searchController.SearchBar;

			resultsTableController.TableView.WeakDelegate = this;
			searchController.SearchBar.WeakDelegate = this;

			DefinesPresentationContext = true;

			if (searchControllerWasActive)
			{
				searchController.Active = searchControllerWasActive;
				searchControllerWasActive = false;

				if (searchControllerSearchFieldWasFirstResponder)
				{
					searchController.SearchBar.BecomeFirstResponder();
					searchControllerSearchFieldWasFirstResponder = false;
				}
			}
		}

		[Export("searchBarSearchButtonClicked:")]
		public virtual void SearchButtonClicked(UISearchBar searchBar)
		{
			searchBar.ResignFirstResponder();
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = new UITableViewCell();
			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			PlaceAutocompleteAPI_Prediction selectedProduct = resultsTableController.FilteredPredictions[indexPath.Row];
			ShowLoadingView("Getting location details ...");

			Task runSync = Task.Factory.StartNew(async (object inputObj) =>
			{
				var placeId = inputObj != null ? inputObj.ToString() : "";

				if (!String.IsNullOrEmpty(placeId))
				{
					var data = await GetPlaceDetails(placeId);

					var zipCode = data.result.address_components.SingleOrDefault(r => r.types.Contains("postal_code"));
					if (zipCode == null)
					{
						HideLoadingView();
						ShowMessageBox("Invalid pickup location", "The location you picked, doesn't have a valid zip code. Please choose another pick-up location.", "OK", null, null);
						return;
					}

					var city = data.result.address_components.SingleOrDefault(r => r.types.Contains("locality"));
					if (city == null)
					{
						HideLoadingView();
						ShowMessageBox("Invalid pickup location", "The location you picked, doesn't have a valid city. Please choose another pick-up location.", "OK", null, null);
						return;
					}

					var street = data.result.address_components.SingleOrDefault(r => r.types.Contains("route"));
					if (street == null)
					{
						HideLoadingView();
						ShowMessageBox("Invalid pickup location", "The location you picked, doesn't have a valid street. Please choose another pick-up location.", "OK", null, null);
						return;
					}

					InvokeOnMainThread(() =>
					{
						traDelegate(data);
						thisController.PopViewController(true);
					});
				}
				HideLoadingView();
			}, selectedProduct.place_id).Unwrap();
		}

		[Export("updateSearchResultsForSearchController:")]
		public virtual void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			var tableController = (ResultsTableController)searchController.SearchResultsController;

			ShowLoadingView("Searching...");
			var searchTerm = searchController.SearchBar.Text.Trim();

			Task runSync = Task.Factory.StartNew(async (object inputObj) =>
			{
				var sTerm = inputObj != null ? inputObj.ToString() : "";
				if (sTerm.Length > 2)
				{
					var searchData = await PerformSearch(sTerm);
					InvokeOnMainThread(() =>
					{
						tableController.FilteredPredictions = searchData;
						tableController.TableView.ReloadData();
					});
				}
				HideLoadingView();
			}, searchTerm).Unwrap();
		}

		private async Task<List<PlaceAutocompleteAPI_Prediction>> PerformSearch(string searchString)
		{
			searchString = searchString.Trim();

			var lResult = LocationHelper.GetLocationResult();
			var data = await GetPlaceAutocomplete(searchString, lResult.Latitude, lResult.Longitude);
			var currentPredictions = data.predictions;

			return currentPredictions;
		}

		public async Task<PlaceDetailsAPI_RootObject> GetPlaceDetails(String placeid)
		{
			PlaceDetailsAPI_RootObject ro = null;
			try
			{
				using (var httpClient = new HttpClient())
				{
					string url = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + placeid + "&key=" + GOOGLE_PLACE_APIKEY;
					var json = await httpClient.GetStringAsync(url);
					ro = JsonConvert.DeserializeObject<PlaceDetailsAPI_RootObject>(json);
				}
			}
			catch (Exception e)
			{
				//TODO: better logging
				Debug.WriteLine("GetPlaceDetails failed: " + e.Message);
			}
			return ro;
		}
		public async Task<PlaceAutocompleteAPI_RootObject> GetPlaceAutocomplete(String input, Double latitude, Double longitude, int radius = 500)
		{
			PlaceAutocompleteAPI_RootObject ro = null;
			try
			{
				using (var httpClient = new HttpClient())
				{
					//&types=geocode
					string url = String.Format("https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&location={1},{2}&radius={3}&components=country:us&key={4}",
						input, latitude, longitude, radius, GOOGLE_PLACE_APIKEY);
					var json = await httpClient.GetStringAsync(url);
					ro = JsonConvert.DeserializeObject<PlaceAutocompleteAPI_RootObject>(json);
				}
			}
			catch (Exception e)
			{
				//TODO: better logging
				Debug.WriteLine("GetPlaceAutocomplete failed: " + e.Message);
			}
			return ro;
		}
	}



	public class ResultsTableController : BaseTableViewController
	{
		public List<PlaceAutocompleteAPI_Prediction> FilteredPredictions { get; set; }

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return FilteredPredictions.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			PlaceAutocompleteAPI_Prediction prediction = FilteredPredictions[indexPath.Row];
			UITableViewCell reusableCell = tableView.DequeueReusableCell(cellIdentifier);

			var cellStyle = UITableViewCellStyle.Default;
			UITableViewCell cell = reusableCell ?? new UITableViewCell(cellStyle, cellIdentifier);
			ConfigureCell(cellStyle, cell, prediction, indexPath.Row);
			return cell;
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.TableFooterView = GetPoweredByGoogle();
		}
	}

	public class BaseTableViewController : UITableViewController
	{
		protected const string cellIdentifier = "cellID";

		public BaseTableViewController()
		{
			Initialize();
		}

		public BaseTableViewController(IntPtr handle) : base(handle)
		{
			Initialize();
		}
		private void Initialize()
		{
			//var button = new UIButton(new CGRect(0, 0, 20, 20));
			////button.SetImage(UIImage.FromBundle("back-ic"), UIControlState.Normal);
			//button.TouchUpInside += (sender, e) => NavigationController.PopViewController(true);
			//NavigationItem.LeftBarButtonItem = new UIBarButtonItem(button);
		}

		protected UIView GetPoweredByGoogle()
		{
			UIImage footerImage = UIImage.FromBundle("powered_by_google_on_white@3x.png");

			UIImageView footerImageView = new UIImageView(footerImage);

			UIView footerView = new UIView(new CGRect(15, 15, footerImageView.Frame.Width, footerImageView.Frame.Height * 2));
			footerView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			footerView.LayoutMargins = new UIEdgeInsets(20, 0, 0, 0);
			footerView.AddSubview(footerImageView);
			return footerView;
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
			InvokeOnMainThread(() =>
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

		protected bool TextFieldShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}

		protected void ConfigureCell(UITableViewCellStyle style, UITableViewCell cell, PlaceAutocompleteAPI_Prediction prediction, int idx)
		{
			if (cell != null)
			{
				switch (style)
				{
					case UITableViewCellStyle.Default:
						cell.TextLabel.Font = UIFont.FromName("Helvetica Light", 14);
						cell.TextLabel.AdjustsFontSizeToFitWidth = true; // gets smaller if it doesn't fit
						cell.TextLabel.MinimumFontSize = 12f; // never gets smaller than this size
						cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
						cell.TextLabel.Text = prediction.description;
						break;
					case UITableViewCellStyle.Subtitle:
						cell.TextLabel.Font = UIFont.FromName("Helvetica Light", 14);
						cell.TextLabel.AdjustsFontSizeToFitWidth = true; // gets smaller if it doesn't fit
						cell.TextLabel.MinimumFontSize = 12f; // never gets smaller than this size
						cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
						cell.TextLabel.Text = prediction.description;


						cell.DetailTextLabel.Font = UIFont.FromName("Helvetica Light", 12);
						cell.DetailTextLabel.Text = String.Join(", ", prediction.types);
						break;
				}
				if (idx % 2 == 0)
				{
					cell.BackgroundColor = new UIColor(217.0f / 255.0f, 217.0f / 255.0f, 217.0f / 255.0f, 1.0f);
				}
				//string detailedStr = string.Format ("{0:C} | {1}", product.IntroPrice, product.YearIntroduced);
				//cell.DetailTextLabel.Text = detailedStr;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
	}
}

