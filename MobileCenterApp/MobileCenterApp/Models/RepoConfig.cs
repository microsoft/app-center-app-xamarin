using System;
using SQLite;

namespace MobileCenterApp
{
	public class RepoConfig
	{
		[PrimaryKey]
		public string Id { get; set; }

		public string Type { get; set; }

		public string State { get; set; }

		public string RepoUrl { get; set; }

		[Indexed]
		public string AppId { get; set; }
	}
}

