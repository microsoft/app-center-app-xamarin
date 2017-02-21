using System;
using SQLite;
using SimpleDatabase;

namespace MobileCenterApp
{
	public class User
	{
		[PrimaryKey]
		public string Id { get; set; }

		public string Email { get; set; }

		[OrderBy]
		public string DisplayName { get; set; }

		[GroupBy]
		public string IndexCharacter { get; set; }

		public string Name { get; set; }

		public string AvatarUrl { get; set; }

		public bool? CanChangePassword { get; set; }
	}
}
