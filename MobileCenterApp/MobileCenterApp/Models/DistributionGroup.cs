using System;
using SQLite;
using SimpleDatabase;
namespace MobileCenterApp
{
	public class DistributionGroup
	{
		[PrimaryKey]
		public string Id { get; set; }

		[Indexed]
		public string AppId { get; set; }

		[OrderBy]
		public string Name { get; set; }

		[GroupBy]
		public string IndexCharacter { get; set; }

	}
}
