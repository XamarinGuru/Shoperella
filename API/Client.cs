using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Shoperella.Model;

namespace Shoperella.API
{
	public class Client
	{
		const string API_BASE = "https://shoperella.fusiondev.co/api/";

		private WebClient GetClient(User user = null)
		{
			var client = new WebClient();
			client.Headers.Add("Accept: application/json");
			client.Headers.Add("Content-Type: application/json");
			if (user != null)
				client.Headers.Add("X-AUTH-TOKEN: " + user.apiToken);
			return client;
		}

		//// <summary>
		/// Login the specified facebookToken and userId.  If the user doesn't exist then they're automatically registered.
		/// </summary>
		/// <param name="facebookToken">Facebook token.</param>
		/// <param name="userId">User identifier.</param>
		/// <returns>The raw JSON from the API</returns>
		public string Login(string facebookToken, string userId)
		{
			var client = GetClient();

			try
			{
				var url = API_BASE + "auth/login.json";
				var data = String.Format("{{ \"facebookId\": \"{0}\" }}", userId);
				var response = client.UploadString(url, data);
				client.Dispose();

				return response;
			}
			catch (Exception e)
			{
				// The call failed, so we want to register the suer
				// TODO: We need to actually check the exception to make sure it was a UserNotFound
				Console.WriteLine("Exception in Login:" + e.Message);
				return Register(facebookToken);
			}
		}

		private string Register(string facebookToken)
		{
			var client = GetClient();

			try
			{
				var url = API_BASE + "auth/register.json";
				var data = String.Format("{{\"mobile_signup\": {{ \"facebookAccessToken\": \"{0}\"}}}}", facebookToken);
				var response = client.UploadString(url, data);

				return response;
			}
			catch (Exception e)
			{
				// The call failed, so we want to register the suer
				// TODO: We need to actually check the exception to make sure it was a UserNotFound
				Console.WriteLine("Exception in Register:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Get User Detail.
		/// </summary>
		/// <returns>User Detail.</returns>
		public string GetUserDetail(User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "auth/session";
				var response = client.DownloadString(url);

				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in GetUserDetail:" + e.Message);
				return null;
			}
		}



		/// <summary>
		/// Creates the wish.
		/// </summary>
		/// <returns>The wish.</returns>
		/// <param name="vendor">query.</param>
		/// <param name="lat">Lat.</param>
		/// <param name="lng">Lng.</param>
		public string CreateWish(string query, double lat, double lng, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "wishes";
				var wishJson = String.Format("{{\"wish\": {{\"query\": \"{0}\", \"latitude\": \"{1}\", \"longitude\": \"{2}\"}}}}", query, lat, lng);
				var response = client.UploadString(url, wishJson);

				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in CreateWish:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Get the wishes.
		/// </summary>
		/// <returns>The wish.</returns>
		/// <param name="vendorID">vendorID.</param>
		public List<Wish> GetWishes(string vendorID)
		{
			var client = GetClient();

			try
			{
				var url = API_BASE + "vendors/" + vendorID + "/assigned/wishes";
				var response = client.DownloadString(url);

				if (response == "")
					return null;

				List<Wish> returnWishes = new List<Wish>();
				var wishesJson = JArray.Parse(response);
				foreach (var wishJson in wishesJson)
				{
					var userJson = JObject.FromObject(wishJson)["user"].ToString();
					var deal = new Wish(JObject.FromObject(wishJson), new User(JObject.Parse(userJson)));
					returnWishes.Add(deal);
				}
				return returnWishes;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in GetWishes:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Gets the vendors.
		/// </summary>
		/// <param name="lat">Lat.</param>
		/// <param name="lng">Lng.</param>
		public List<Vendor> GetVendors(double lat, double lng)
		{
			var client = GetClient();

			try
			{
				var url = API_BASE + String.Format("vendors?coords={0},{1}", lat.ToString(), lng.ToString());
				var response = client.DownloadString(url);
				var vendorsJson = JArray.Parse(response);

				List<Vendor> vendors = new List<Vendor>();
				foreach (var vendorJson in vendorsJson)
				{
					var vendor = new Vendor(JObject.FromObject(vendorJson));
					vendors.Add(vendor);
				}

				return vendors;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in GetVendors:" + e.Message);
				return null;
			}
		}

		public string GetVendor(Vendor vendor) 
		{
			var client = GetClient();

			try
			{
				var url = API_BASE + String.Format("vendors/{0}", vendor.ID);
				var response = client.DownloadString(url);

				// TODO: Sanity check the response to make sure we actually got a vendor back.
				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in GetVendor:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Creates the auto response.
		/// </summary>
		/// <returns>The response's JSON.</returns>
		/// <param name="name">Name.</param>
		/// <param name="address">Address.</param>
		/// <param name="zip">Zip.</param>
		public Deal CreateAutoResponse(Vendor vendor, string title, string caption, string description, string expiresAt, User user, string autoResponseId = "", string update = "0")
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "vendor/autoresponse";

				var autoResponseJson = String.Format("{{\"vendorId\": \"{0}\", \"title\": \"{1}\", \"caption\": \"{2}\", \"description\": \"{3}\", \"expiresAt\": \"{4}\"}}", vendor.ID, title, caption, description, expiresAt);

				var response = client.UploadString(url, autoResponseJson);
				var autoResponse = new Deal(JObject.Parse(response), vendor);

				return autoResponse;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in CreateAutoResponse:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Creates the deal.
		/// </summary>
		/// <returns>The deal.</returns>
		/// <param name="vendor">Vendor.</param>
		/// <param name="title">Title.</param>
		/// <param name="caption">Caption.</param>
		/// <param name="description">Description.</param>
		/// <param name="expiresAt">Expires at.</param>
		/// <param name="user">User.</param>
		public Deal CreateDeal(Vendor vendor, string title, string caption, string description, string expiresAt, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "vendors/" + vendor.ID + "/deals";
				var dealJson = String.Format("{{\"title\": \"{0}\", \"caption\": \"{1}\", \"description\": \"{2}\", \"expiresAt\": \"{3}\"}}", title, caption, description, expiresAt);
				var response = client.UploadString(url, dealJson);

				var deal = new Deal(JObject.Parse(response), vendor);
				return deal;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in CreateDeal:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Get all deals.
		/// </summary>
		/// <returns>The deals.</returns>
		public List<Deal> GetDeals(User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "deals";
				var response = client.DownloadString(url);

				if (response == "")
					return null;

				var dealsJson = JArray.Parse(response);

				List<Deal> deals = new List<Deal>();
				foreach (var dealJson in dealsJson)
				{
					var deal = new Deal(JObject.FromObject(dealJson), new Vendor(JObject.FromObject(dealJson["vendor"])));
					deals.Add(deal);
				}

				return deals;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in CreateDeal:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Search deals.
		/// </summary>
		/// <returns>The deals.</returns>
		public List<Deal> SearchDeals(string keyword, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "deals/search/" + keyword;
				var response = client.DownloadString(url);

				if (response == "")
					return null;

				var dealsJson = JArray.Parse(response);

				List<Deal> deals = new List<Deal>();
				foreach (var dealJson in dealsJson)
				{
					var deal = new Deal(JObject.FromObject(dealJson), new Vendor(JObject.FromObject(dealJson["vendor"])));
					deals.Add(deal);
				}

				return deals;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in CreateDeal:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Creates the offer.
		/// </summary>
		/// <returns>The offer.</returns>
		/// <param name="vendor">Vendor.</param>
		/// <param name="title">Title.</param>
		/// <param name="caption">Caption.</param>
		/// <param name="description">Description.</param>
		/// <param name="expiresAt">Expires at.</param>
		/// <param name="user">User.</param>
		public Offer CreateOffer(string vendorID, string wishID, string title, string caption, string description, string expiresAt, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "create/offer";
				var wishJson = String.Format("{{\"vendorId\": \"{0}\", \"wishId\": \"{1}\", \"userId\": \"{2}\", \"title\": \"{3}\", \"caption\": \"{4}\", \"description\": \"{5}\", \"expiresAt\": \"{6}\"}}", vendorID, wishID, user.id, title, caption, description, expiresAt);
				var response = client.UploadString(url, wishJson);

				var offer = new Offer(JObject.Parse(response), null);
				return offer;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in CreateOffer:" + e.Message);
				return null;
			}
		}


		/// <summary>
		/// Get the user offers.
		/// </summary>
		/// <returns>The offers.</returns>
		public List<Offer> GetUserOffers(User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "offers/user";

				var response = client.DownloadString(url);

				if (response == "")
					return null;

				var offersJson = JArray.Parse(response);

				List<Offer> offers = new List<Offer>();
				foreach (var offerJson in offersJson)
				{
					var offer = new Offer(JObject.FromObject(offerJson), new Vendor(JObject.FromObject(offerJson["vendor"])));
					offers.Add(offer);
				}

				return offers;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in GetUserOffers:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Dismiss the user offers.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The offers.</returns>
		public Offer DismissUserOffer(string offerID, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "offer/dismiss/" + offerID;

				var response = client.DownloadString(url);

				if (response == "")
					return null;

				var offerObj = JObject.Parse(response);
				var vendorObj = JObject.Parse(offerObj["vendor"].ToString());
				var offer = new Offer(offerObj, new Vendor(vendorObj));

				return offer;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in DismissUserOffer:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Get the user favorites.
		/// </summary>
		/// <returns>The offers.</returns>
		public List<Deal> GetUserFavorites(User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "favorites";

				var response = client.DownloadString(url);

				if (response == "")
					return null;

				var favoritesJson = JArray.Parse(response);

				if (favoritesJson.Count == 0)
					return null;
				
				var favoritesArr = JArray.Parse(favoritesJson[0]["favorite"].ToString());

				List<Deal> favorites = new List<Deal>();
				foreach (var favoriteJson in favoritesArr)
				{
					var favorite = new Deal(JObject.FromObject(favoriteJson), new Vendor(JObject.FromObject(favoriteJson["vendor"])));
					favorites.Add(favorite);
				}

				return favorites;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in GetUserFavorites:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Get the vendor offers.
		/// </summary>
		/// <returns>The offers.</returns>
		public List<Offer> GetVendorOffers(Vendor vendor, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "offers/vendor/" + vendor.ID;

				var response = client.DownloadString(url);

				if (response == "")
					return null;

				var offersJson = JArray.Parse(response);

				List<Offer> offers = new List<Offer>();
				foreach (var offerJson in offersJson)
				{
					var offer = new Offer(JObject.FromObject(offerJson), null);
					offers.Add(offer);
				}

				return offers;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in GetVendorOffers:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Delete the vendor offer.
		/// </summary>
		/// <returns>The offers.</returns>
		public Offer DeleteOffer(string offerID, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "offers/" + offerID;

				var response = client.UploadString(url, "DELETE", "");

				var offer = new Offer(JObject.Parse(response), null);
					
				return offer;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in DeleteOffer:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Renew the vendor offer.
		/// </summary>
		/// <returns>The offers.</returns>
		public Offer RenewOffer(string offerID, User user)
		{
			var client = GetClient(user);

			try
			{
				var url = API_BASE + "offers/" + offerID + "/extend";

				var response = client.UploadString(url, "PUT", "");

				var offer = new Offer(JObject.Parse(response), null);

				return offer;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in RenewOffer:" + e.Message);
				return null;
			}
		}



		#region HttpClient
		/// <summary>
		/// Send PushDeviceToken.
		/// </summary>
		/// <returns>User Detail.</returns>
		public async Task<string> SendPushDeviceToken(string deviceIdentifier, User user)
		{
			var url = API_BASE + "user/notifications";

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var deviceType = new StringContent("ios");
				content.Add(deviceType, "deviceType");

				var deviceIdentifierStringContent = new StringContent(deviceIdentifier);
				content.Add(deviceIdentifierStringContent, "deviceIdentifier");

				var response = await client.PostAsync(url, content);
				return response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in SendPushDeviceToken:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Update the vendor logo.
		/// </summary>
		/// <returns>The vendor logo.</returns>
		public async Task<string> UploadVendorLogo(string vendorID, byte[] logoData, User user)
		{
			var url = API_BASE + "vendor/add/logo";

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var imgFileContent = GetHttpClientFileContent(logoData, "logo");
				content.Add(imgFileContent, "logo");

				var vendorIdStringContent = new StringContent(vendorID);
				content.Add(vendorIdStringContent, "vendorId");

				var response = await client.PostAsync(url, content);
				return response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in UploadVendorLogo:" + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Creates a new vendor.
		/// </summary>
		/// <returns>The vendor's JSON.</returns>
		/// <param name="name">Name.</param>
		/// <param name="address">Address.</param>
		/// <param name="zip">Zip.</param>
		public async Task<string> CreateVendor(byte[] logoData, string name, string city, string street, string zip, User user)
		{
			var url = API_BASE + "vendors";

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var imgFileContent = GetHttpClientFileContent(logoData, "logo");
				content.Add(imgFileContent, "logo");

				var nameStringContent = new StringContent(name);
				content.Add(nameStringContent, "name");

				var street1StringContent = new StringContent(city);
				content.Add(street1StringContent, "street1");

				var street2StringContent = new StringContent(street);
				content.Add(street2StringContent, "street2");

				var zipStringContent = new StringContent(zip);
				content.Add(zipStringContent, "zip");


				var response = await client.PostAsync(url, content);
				return response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in CreateVendor:" + e.Message);
				return null;
			}
		}

		/// Update the vendor profile.
		/// </summary>
		/// <returns>The vendor profile.</returns>
		/// <param name="vendorId">vendorId.</param>
		/// <param name="logo">logo.</param>
		/// <param name="name">name.</param>
		/// <param name="street1">street1.</param>
		/// <param name="street2">street2.</param>
		/// <param name="zip">zip.</param>
		public async Task<string> UpdateVendor(string vendorID, byte[] logoData, string name, string city, string street, string zip, User user)
		{
			var url = API_BASE + "vendors/update/" + vendorID;

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var imgFileContent = GetHttpClientFileContent(logoData, "logo");
				content.Add(imgFileContent, "logo");

				var nameStringContent = new StringContent(name);
				content.Add(nameStringContent, "name");

				var street1StringContent = new StringContent(city);
				content.Add(street1StringContent, "street1");

				var street2StringContent = new StringContent(street);
				content.Add(street2StringContent, "street2");

				var zipStringContent = new StringContent(zip);
				content.Add(zipStringContent, "zip");


				var response = await client.PostAsync(url, content);
				return response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in UpdateVendor:" + e.Message);
				return null;
			}
		}

		/// Add favorite to vendor.
		/// </summary>
		/// <returns>The user profile.</returns>
		/// <param name="dealId">dealId.</param>
		public async Task<List<Deal>> AddToFavorite(string offerID, User user)
		{
			var url = API_BASE + "favorite/add";

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var dealIDStringContent = new StringContent(offerID);
				content.Add(dealIDStringContent, "dealId");

				var response = await client.PostAsync(url, content);

				var jsonResponse = response.Content.ReadAsStringAsync().Result;
				if (jsonResponse == "")
					return null;

				var jsonObject = JObject.Parse(jsonResponse);
				var dealsJson = JArray.Parse(jsonObject["favorite"].ToString());

				List<Deal> deals = new List<Deal>();
				foreach (var dealJson in dealsJson)
				{
					var deal = new Deal(JObject.FromObject(dealJson), new Vendor(JObject.FromObject(dealJson["vendor"])));
					deals.Add(deal);
				}
				return deals;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in AddToFavorite:" + e.Message);
				return null;
			}
		}

		/// Remove favorite to vendor.
		/// </summary>
		/// <returns>The user profile.</returns>
		/// <param name="dealId">dealId.</param>
		public async Task<List<Deal>> RemoveFromFavorite(string dealId, User user)
		{
			var url = API_BASE + "favorite/remove";

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var dealIDStringContent = new StringContent(dealId);
				content.Add(dealIDStringContent, "dealId");

				var response = await client.PostAsync(url, content);

				var jsonResponse = response.Content.ReadAsStringAsync().Result;
				if (jsonResponse == "")
					return null;

				var jsonObject = JObject.Parse(jsonResponse);
				var dealsJson = JArray.Parse(jsonObject["favorite"].ToString());

				List<Deal> deals = new List<Deal>();
				foreach (var dealJson in dealsJson)
				{
					var deal = new Deal(JObject.FromObject(dealJson), new Vendor(JObject.FromObject(dealJson["vendor"])));
					deals.Add(deal);
				}
				return deals;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in RemoveFromFavorite:" + e.Message);
				return null;
			}
		}

		/// Redeem offer
		/// </summary>
		/// <returns>The Offer.</returns>
		/// <param name="dealId">offerID.</param>
		public async Task<string> RedeemOffer(string offerID, User user)
		{
			var url = API_BASE + "redeem/offer";

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var offerIDStringContent = new StringContent(offerID);
				content.Add(offerIDStringContent, "offerId");

				var response = await client.PostAsync(url, content);

				var jsonResponse = response.Content.ReadAsStringAsync().Result;
				if (jsonResponse == "")
					return null;

				var jsonObject = JObject.Parse(jsonResponse);

				return jsonObject["redeemed_at"].ToString();
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in RemoveFromFavorite:" + e.Message);
				return null;
			}
		}

		/// Redeem deal
		/// </summary>
		/// <returns>The Offer.</returns>
		/// <param name="dealId">offerID.</param>
		public async Task<string> RedeemDeal(string dealID, User user)
		{
			var url = API_BASE + "redeem/deal";

			var client = new HttpClient();
			var content = GetHttpClientContent(user);

			try
			{
				var dealIDStringContent = new StringContent(dealID);
				content.Add(dealIDStringContent, "dealId");

				var response = await client.PostAsync(url, content);

				var jsonResponse = response.Content.ReadAsStringAsync().Result;
				if (jsonResponse == "")
					return null;

				var jsonObject = JObject.Parse(jsonResponse);

				return jsonObject["redeemed_at"].ToString();
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in RemoveFromFavorite:" + e.Message);
				return null;
			}
		}







		private MultipartFormDataContent GetHttpClientContent(User user = null)
		{
			var content = new MultipartFormDataContent();

			if (user != null)
				content.Headers.Add("X-AUTH-TOKEN", user.apiToken);
			
			return content;
		}
		private ByteArrayContent GetHttpClientFileContent(byte[] fileData, string fileName)
		{
			if (fileData == null)
				return new StringContent(string.Empty);
			
			var fileContent = new ByteArrayContent(fileData);
			fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
			fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
			{
				Name = fileName,
				FileName = fileName + ".jpg",
			};

			return fileContent;
		}
		#endregion

	}
}

