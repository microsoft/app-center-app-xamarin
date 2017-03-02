using System;
using SQLite;
using SimpleAuth;

namespace MobileCenterApp
{
	public enum ExceptionPlatform
	{
		Ios,
		Android,
		Xamarin,
		ReactNative,
		Other
	}

	public class StackTrace
	{
		[PrimaryKey]
		public string CrashGroupId { get; set; }
		public string Title { get; set; }
		public string Reason { get; set; }

		[Ignore]
		public ThreadModel[] Threads { get; set; }

		//Backing fields used to store the data in Sqlite;
		public string ThreadModelString
		{
			get { return Threads.ToJson(); }
			set { Threads = value.ToObject<ThreadModel[]>(); }
		}

		[Ignore]
		public ExceptionModel Exception { get; set; }

		//Backing fields used to store the data in Sqlite;
		public string ExceptionString
		{
			get { return Exception.ToJson(); }
			set { Exception = value.ToObject<ExceptionModel>();  }
		}
	}
}
