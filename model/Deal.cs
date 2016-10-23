using System;
using Newtonsoft.Json.Linq;

namespace Shoperella.Model
{
	public class Deal
	{
		public string ID { get; private set; }
		public string Title { get; private set; }
		public string Caption { get; private set; }
		public string Description { get; private set; }
		public DateTime ExpiresAt { get; private set; }
		public Vendor Vendor { get; private set; }

		public Deal(JObject dealJson, Vendor vendor)
		{
			if (dealJson.GetValue("id") != null)
				this.ID = dealJson["id"].ToString();
			else
				this.ID = dealJson["ID"].ToString();


			if (dealJson.GetValue("title") != null)
				this.Title = dealJson["title"].ToString();
			else
				this.Title = dealJson["Title"].ToString();


			if (dealJson.GetValue("caption") != null)
				this.Caption = dealJson["caption"].ToString();
			else
				this.Caption = dealJson["Caption"].ToString();


			if (dealJson.GetValue("description") != null)
				this.Description = dealJson["description"].ToString();
			else
				this.Description = dealJson["Description"].ToString();


			if (dealJson.GetValue("expires_at") != null)
				this.ExpiresAt = DateTime.Parse(dealJson["expires_at"].ToString());
			else if (dealJson.GetValue("expiresAt") != null)
				this.ExpiresAt = DateTime.Parse(dealJson["expiresAt"].ToString());
			else
				this.ExpiresAt = DateTime.Parse(dealJson["ExpiresAt"].ToString());
			
			this.Vendor = vendor;
		}
	}
}

