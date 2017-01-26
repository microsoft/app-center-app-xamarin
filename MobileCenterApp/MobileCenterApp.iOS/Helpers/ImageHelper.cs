using System;
using MobileCenterApp.Resources;
using System.IO;
using MobileCenterApp.Data;
using UIKit;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public static class ImageHelper
	{
		public static void SaveImage (string svg, double size, string cachedPath)
		{
			var svgStream = new StreamReader(ResourceHelper.GetEmbeddedResourceStream (svg));
			var image = svgStream.LoadImageFromSvgStream (size);
			image.AsPNG ().Save (cachedPath, false);
		}

		public static string GetCachedImagedName (string svg, double size)
		{
			var name = Path.GetFileNameWithoutExtension (svg);
			string scaleModifier = NGraphicsExtensions.Scale > 1 ? $"@{NGraphicsExtensions.Scale}x" :"";
			var cachedName = $"{name}-{size}{scaleModifier}.png";
			var cachedImage = Path.Combine (Locations.ImageCache, cachedName);
			return cachedImage;
		}

		public static async Task<UIImage> GetImage(string svg, double size,bool cached)
		{
			if (cached)
			{
				var svgStream = new StreamReader(ResourceHelper.GetEmbeddedResourceStream(svg));
				var image = svgStream.LoadImageFromSvgStream(size);
				return image;
			}
			var imagePath = GetCachedImagedName(svg, size);
			if (!File.Exists(imagePath))
				await Task.Run(() => SaveImage(svg, size, imagePath));
			return UIImage.FromFile(imagePath);
		}


	}
}

