using System;
using SimpleDatabase;
using SQLite;
namespace MobileCenterApp
{
	public class Branch
	{
		public Branch()
		{
		}

		[PrimaryKey]
		public string Id {
			get { return $"{AppId} - {Name}"; }
			set { }
		}

		public AppClass App
		{
			get { return Database.Main.GetObject<AppClass>(AppId); }
		}

		[Indexed]
		public string AppId { get; set; }

		[OrderBy]
		public string Name { get; set; }

		[GroupBy]
		public string IndexCharacter { get; set; }

		public int LastBuildId { get; set; }

		public string LastCommitId { get; set; }

		public string BuildStatus { get; set; }
	}
}
