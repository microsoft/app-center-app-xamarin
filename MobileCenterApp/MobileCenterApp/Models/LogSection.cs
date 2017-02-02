using System;
using System.Collections.Generic;

namespace MobileCenterApp
{
	public class LogSection : LogLine
	{
		public LogSection()
		{
		}

		public List<LogSection> Sections { get; set; } = new List<LogSection>();
		public List<string> Lines { get; set; } = new List<string>();
	}
}
