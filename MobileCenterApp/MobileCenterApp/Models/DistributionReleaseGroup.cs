using System;
using SQLite;
using SimpleDatabase;
namespace MobileCenterApp
{
	public class DistributionReleaseGroup
	{
		[PrimaryKey]
		public string Id { 
			get { return $"{DistributionId} - {ReleaseId}";}
			set { }
		}
		[Indexed]
		public string DistributionId { get; set; }

		public string ReleaseId { get; set; }

		[OrderBy]
		public DateTime ReleaseDate { get; set; }

		[Ignore]
		public Release Release
		{
			get
			{
				return Database.Main.GetObject<Release>(ReleaseId);
			}
			set
			{
				ReleaseId = value.ReleaseId;
				ReleaseDate = value.UploadedAt;
			}
		}
	}
}
