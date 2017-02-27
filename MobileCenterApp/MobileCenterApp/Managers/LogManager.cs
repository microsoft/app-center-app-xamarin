using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Azure.Mobile.Analytics;
using System.IO;
namespace MobileCenterApp
{
	public class LogManager
	{
		public static LogManager Shared { get; set; } = new LogManager();

		public void LogEvent(string evt, Dictionary<string, string> data = null)
		{
			Analytics.TrackEvent(evt, data);
		}

		public void UserLoggedIn(string username)
		{
			LogEvent("Login", new Dictionary<string, string> { { "Username", username } });
		}

		internal void Report(Exception ex, [CallerMemberName] string memberName = "",
							   [CallerFilePath] string sourceFilePath = "",
							   [CallerLineNumber] int sourceLineNumber = 0)
		{
			if (ex.Data.Contains("HttpContent"))
			{
				Console.WriteLine(ex.Data["HttpContent"]);
			}
			else
				Console.WriteLine(ex);
			var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogEvent("Exception", new Dictionary<string, string>
			{
				{"FileName",fileName},
				{"Message",ex.Message},
				{"Line Number",sourceLineNumber.ToString()},
				{"Member Name",memberName}
			});
		}

		public void PageView(string title,Dictionary<string,string> data = null)
		{
			if (data == null)
				data = new Dictionary<string, string> ();
			data["Page Title"] = title;
			LogEvent("Page View", data);
		}

	}
}
