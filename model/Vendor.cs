using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Shoperella.Model
{
	public class Vendor
	{
		public string ID { get; private set; }
		public string Name { get; private set; }
		public string Address { get; private set; }
		public string City { get; private set; }
		public string State { get; private set; }
		public string Zip { get; private set; }
		public double Latitude { get; private set; }
		public double Longitude { get; private set; }
		public double Distance { get; set; }
		public string ImagePath { get; private set; }
		public List<Deal> Deals { get; private set; }

		public Vendor(JObject vendorJson)
		{
			//this.ID = vendorJson["id"].ToString();
			if (vendorJson.GetValue("ID") != null)
				this.ID = vendorJson["ID"].ToString();
			else
				this.ID = vendorJson["id"].ToString();
			
			if (vendorJson.GetValue("Name") != null)
				this.Name = vendorJson["Name"].ToString();
			else
				this.Name = vendorJson["name"].ToString();
			
			if (vendorJson.GetValue("Street1") != null)
				this.Address = vendorJson["Street1"].ToString();
			else if (vendorJson.GetValue("street1") != null)
				this.Address = vendorJson["street1"].ToString();
			else if (vendorJson.GetValue("Address") != null)
				this.Address = vendorJson["Address"].ToString();
			else
				this.Address = vendorJson["address"].ToString();
			
			if (vendorJson.GetValue("City") != null)
				this.City = vendorJson["City"].ToString();
			else
				this.City = vendorJson["city"].ToString();
			
			if (vendorJson.GetValue("State") != null)
				this.State = vendorJson["State"].ToString();
			else
				this.State = vendorJson["state"].ToString();
			
			if (vendorJson.GetValue("Zip") != null)
				this.Zip = vendorJson["Zip"].ToString();
			else
				this.Zip = vendorJson["zip"].ToString();
			
			if (vendorJson.GetValue("Latitude") != null)
				this.Latitude = double.Parse(vendorJson["Latitude"].ToString());
			else
				this.Latitude = double.Parse(vendorJson["latitude"].ToString());
			
			if (vendorJson.GetValue("Longitude") != null)
				this.Latitude = double.Parse(vendorJson["Longitude"].ToString());
			else
				this.Latitude = double.Parse(vendorJson["longitude"].ToString());

			if (vendorJson.GetValue("image_path") != null)
				this.ImagePath = vendorJson["image_path"].ToString();
			else if (vendorJson.GetValue("imagePath") != null)
				this.ImagePath = vendorJson["imagePath"].ToString();
			else
				this.ImagePath = vendorJson["ImagePath"].ToString();
			
			Deals = new List<Deal>();
			if (vendorJson.GetValue("deals") != null)
				SetDeals(JArray.FromObject(vendorJson.GetValue("deals")));
		
		}

		public void SetDeals(JArray dealsJson)
		{
			foreach (var dealJson in dealsJson)
			{
				var deal = new Deal(JObject.FromObject(dealJson), this);
				Deals.Add(deal);
			}

		}


	}
}

