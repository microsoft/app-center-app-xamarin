using System;
using System.IO;
using MobileCenterApp;

namespace MobileCenterApp
{
	public static class Images
	{
		public static string GetCachedImagePath (string svg, double size)
		{
			var cachedImage = ImageHelper.GetCachedImagedName (svg, size);
			if (File.Exists (cachedImage))
				return cachedImage;
			ImageHelper.SaveImage (svg, size, cachedImage);
			return cachedImage;
		}

		public const string LoginHeaderLogo = "login-header-logo.svg";
		public const string GettingStartedHeader = "overview.svg";
		public const string GettingStartedPageIcon = "app-overview.svg";
		public const string BuildPageIcon = "app-build.svg";
		public const string TestPageIcon = "app-test.svg";
		public const string DistributePageIcon = "app-distribute.svg";
		public const string CrashesPageIcon = "app-crashes.svg";
		public const string AnalyticsPageIcon = "app-analytics.svg";
	}
}

