using System;
using SQLite;
namespace MobileCenterApp
{
	public enum ReasonStackFrameLanguage
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

	public class ReasonStackFrame
	{
		[PrimaryKey]
		public string CrashGroupId { get; set; }

		[Indexed]
		public string AppId { get; set; }

		public string ClassName { get; set; }

		public string Method { get; set; }

		public bool? ClassMethod { get; set; }

		public string File { get; set; }

		public int? Line { get; set; }

		public bool? AppCode { get; set; }

		public string FrameworkName { get; set; }

		public string CodeFormatted { get; set; }

		public ReasonStackFrameLanguage? Language { get; set; }

		public string MethodParams { get; set; }

	}
}
