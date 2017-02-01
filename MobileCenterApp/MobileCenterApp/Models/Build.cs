using System;
using SQLite;
using SimpleDatabase;
namespace MobileCenterApp
{
	public class Build
	{
		[PrimaryKey]
		public int Id { get; set; }

		public string BuildNumber { get; set; }

		public string QueueTime { get; set; }

		public string StartTime { get; set; }

		public string FinishTime { get; set; }

		[OrderBy]
		public string LastChangedDate { get; set; }

		public string Status { get; set; }

		public string Result { get; set; }

		[Indexed]
		public string SourceBranch { get; set; }

		public string SourceVersion { get; set; }

		[Indexed]
		public string AppId { get; set; }

		[Indexed]
		public string AppIdBranchId
		{
			get { return $"{AppId} - {SourceBranch}"; }
			set { }
		}

		public CommitClass LastCommit => Database.Main.GetObject<CommitClass>(SourceVersion);

		public string DisplayText => $"Build:{BuildNumber} - {LastCommit?.Message}";
	}
}
