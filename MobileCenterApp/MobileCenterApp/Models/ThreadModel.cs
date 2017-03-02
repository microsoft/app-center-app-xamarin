using System;
namespace MobileCenterApp
{
	public class ThreadModel
	{
		public string Title { get; set; }

		public StackFrame[] Frames { get; set; }

		public ExceptionModel Exception { get; set; }

		public bool? Relevant { get; set; }

		public ExceptionPlatform? Platform { get; set; }
	}
}
