using System;
using Newtonsoft.Json.Linq;

namespace Shoperella.Model
{
	public class User
	{

		public string id { get; private set; }
		public string facebookID { get; private set; }
		public string username { get; private set; }
		public string name { get; private set; }
		public string photoURL { get; private set; }
		public string email { get; private set; }
		public string apiToken { get; private set; }
		public string created { get; private set; }

		public User(JObject userJson)
		{
			id = userJson["id"].ToString();
			username = userJson["username"].ToString();
			name = userJson["name"].ToString();
			email = userJson["email"].ToString();
			created = userJson["created"].ToString();

			if (userJson.GetValue("facebook_id") != null)
				this.facebookID = userJson["facebook_id"].ToString();
			else if (userJson.GetValue("facebookId") != null)
				this.facebookID = userJson["facebookId"].ToString();
			else
				this.facebookID = userJson["facebookID"].ToString();
			

			if (userJson.GetValue("profile_photo_url") != null)
				this.photoURL = userJson["profile_photo_url"].ToString();
			else if (userJson.GetValue("profilePhotoUrl") != null)
				this.photoURL = userJson["profilePhotoUrl"].ToString();
			else
				this.photoURL = userJson["photoURL"].ToString();
			

			if (userJson.GetValue("api_token") != null)
				this.apiToken = userJson["api_token"].ToString();
			else
				this.apiToken = userJson["apiToken"].ToString();
		}

	}
}

