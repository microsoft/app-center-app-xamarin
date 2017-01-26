using System;
using SQLite;

namespace MobileCenterApp
{
	public class Owner
	{
		[PrimaryKey]
		public string Id { get; set; }

		public string AvatarUrl { get; set; }

		[Newtonsoft.Json.JsonProperty("email")]
		public string Email { get; set; }

		[Newtonsoft.Json.JsonProperty("display_name")]
		public string DisplayName { get; set; }

		[Newtonsoft.Json.JsonProperty("name")]
		public string Name { get; set; }

		[Newtonsoft.Json.JsonProperty("type")]
		public string Type { get; set; }
	}
}
