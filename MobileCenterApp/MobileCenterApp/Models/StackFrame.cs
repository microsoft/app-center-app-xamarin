using System;
namespace MobileCenterApp
{
	public enum StackFrameLanguage
	{
		JavaScript,
		CSharp,
		ObjectiveC,
		ObjectiveCpp,
		Cpp,
		C,
		Swift,
		Java,
		Unknown
	}
	public class StackFrame
	{
		public string Address { get; set; }

		public string ClassName { get; set; }

		public string Method { get; set; }

		public bool? ClassMethod { get; set; }

		public string File { get; set; }

		public int? Line { get; set; }

		public bool AppCode { get; set; }

		public string FrameworkName { get; set; }

		public string CodeRaw { get; set; }

		public string CodeFormatted { get; set; }

		public StackFrameLanguage? Language { get; set; }

		public bool? Relevant { get; set; }

		public string MethodParams { get; set; }
	}
}
