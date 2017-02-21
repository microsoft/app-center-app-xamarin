using System;
using SQLite;
namespace MobileCenterApp
{
	public class Tester : User
	{
		[Indexed]
		public string DistributionId { get; set; }

		public bool? InvitePending { get; set; }
	}
}
