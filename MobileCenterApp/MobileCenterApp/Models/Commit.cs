using System;
using SQLite;
namespace MobileCenterApp
{
	public class Commit
	{
		[PrimaryKey]
		public string Sha { get; set; }

		public string Url { get; set; }

		[Indexed]
		public string AppId { get; set; }
	}
}
