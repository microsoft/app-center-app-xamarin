using System;
using SQLite;

namespace MobileCenterApp
{
	public class User
	{
		[PrimaryKey]
		public string Id { get; set; }


		public string Email { get; set; }

		public string DisplayName { get; set; }


		public string Name { get; set; }


		public string AvatarUrl { get; set; }


		public bool CanChangePassword { get; set; }
	}
}
