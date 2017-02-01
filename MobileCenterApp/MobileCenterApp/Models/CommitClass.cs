using System;
using SQLite;
namespace MobileCenterApp
{
	public class CommitClass
	{
		[PrimaryKey]
		public string Sha { get; set; }

		public string Url { get; set; }

		[Indexed]
		public string AppId { get; set; }

		public string Message { get; set; }
	}
}
