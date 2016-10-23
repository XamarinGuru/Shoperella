using System;
using Newtonsoft.Json.Linq;

namespace Shoperella.Model
{
	public class Wish
	{
		public string ID { get; private set; }
		public string Query { get; private set; }
		public string Caption { get; private set; }
		public string Description { get; private set; }
		public double Latitude { get; private set; }
		public double Longitude { get; private set; }
		public Offer Offer { get; private set; }
		public User User { get; private set; }

		public Wish(JObject wishJson, User user)
		{
			if (wishJson.GetValue("id") != null)
				this.ID = wishJson["id"].ToString();
			else
				this.ID = wishJson["ID"].ToString();


			if (wishJson.GetValue("query") != null)
				this.Query = wishJson["query"].ToString();
			else
				this.Query = wishJson["Query"].ToString();
		

			if (wishJson.GetValue("Latitude") != null)
				this.Latitude = double.Parse(wishJson["Latitude"].ToString());
			else
				this.Latitude = double.Parse(wishJson["latitude"].ToString());

			if (wishJson.GetValue("Longitude") != null)
				this.Longitude = double.Parse(wishJson["Longitude"].ToString());
			else
				this.Longitude = double.Parse(wishJson["longitude"].ToString());

			this.Offer = null;
			this.User = user;
		}
	}
}

