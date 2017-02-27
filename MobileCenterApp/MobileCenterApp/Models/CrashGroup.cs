using System;
using SQLite;
namespace MobileCenterApp
{
	public class CrashGroup
	{
		[PrimaryKey]
		public string Id { get; set; }

		[Indexed]
		public string AppId { get; set; }

		public string DisplayId { get; set; }

		public string AppVersion { get; set; }

		public string Build { get; set; }

		[Indexed]
		public CrashGroupStatus Status { get; set; }

		public int Count { get; set; }

		public int? ImpactedUsers { get; set; }

		public DateTime FirstOccurrence { get; set; }

		public DateTime LastOccurrence { get; set; }

		public string Exception { get; set; }

		public string ErrorReason { get; set; }

		[IgnoreAttribute]
		public ReasonStackFrame ReasonFrame => Database.Main.GetObject<ReasonStackFrame>(Id);

		public bool Fatal { get; set; }
	}
}
