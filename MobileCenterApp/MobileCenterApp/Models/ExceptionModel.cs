using System;
namespace MobileCenterApp
{
	public class ExceptionModel
	{
		public string Reason { get; set; }

		public string Type { get; set; }

		public StackFrame[] Frames { get; set; }

		public bool? Relevant { get; set; }

		public ExceptionModel[] InnerExceptions { get; set; }

		public ExceptionPlatform? Platform { get; set; }
	}
}
