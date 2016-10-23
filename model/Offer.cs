using System;
using Newtonsoft.Json.Linq;

namespace Shoperella.Model
{
	public class Offer
	{
		public string ID { get; private set; }
		public string Title { get; private set; }
		public string Caption { get; private set; }
		public string Description { get; private set; }
		public DateTime ExpiresAt { get; private set; }
		public Vendor Vendor { get; private set; }
		public User User { get; private set; }

		public Offer(JObject offerJson, Vendor vendor)
		{
			if (offerJson.GetValue("id") != null)
				this.ID = offerJson["id"].ToString();
			else
				this.ID = offerJson["ID"].ToString();


			if (offerJson.GetValue("title") != null)
				this.Title = offerJson["title"].ToString();
			else
				this.Title = offerJson["Title"].ToString();


			if (offerJson.GetValue("caption") != null)
				this.Caption = offerJson["caption"].ToString();
			else
				this.Caption = offerJson["Caption"].ToString();


			if (offerJson.GetValue("description") != null)
				this.Description = offerJson["description"].ToString();
			else
				this.Description = offerJson["Description"].ToString();


			if (offerJson.GetValue("expiresAt") != null)
				this.ExpiresAt = DateTime.Parse(offerJson["expiresAt"].ToString());
			else if (offerJson.GetValue("expires_at") != null)
				this.ExpiresAt = DateTime.Parse(offerJson["expires_at"].ToString());
			else
				this.ExpiresAt = DateTime.Parse(offerJson["ExpiresAt"].ToString());
			
			this.Vendor = vendor;
			//this.User = user;
		}
	}
}
