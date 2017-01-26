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
	}
}

