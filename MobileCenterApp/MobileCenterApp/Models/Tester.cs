using System;
using SQLite;
using SimpleDatabase;

namespace MobileCenterApp
{
	public class Tester : BaseModel
	{
		[PrimaryKey]
		public string Id
		{
			get { return $"{DistributionId} - {UserId}"; }
			set { }
		}

		[Indexed]
		public string DistributionId { get; set; }

		[Indexed]
		public string AppId { get; set; }

		public bool? InvitePending { get; set; }

		[Indexed]
		public string UserId { get; set; }

		[GroupBy]
		public string IndexCharacter { get; set; }

		[OrderBy]
		public string DisplayName { get; set; }

		[Ignore]
		public User User
		{
			get
			{
				return Database.Main.GetObject<User>(UserId);
			}
			set
			{
				UserId = value.Id;
				DisplayName = value.DisplayName;
				IndexCharacter = BaseModel.GetIndexChar(DisplayName);
			}
		}
	}
}
