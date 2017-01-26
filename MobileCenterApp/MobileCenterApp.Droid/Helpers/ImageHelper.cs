using System;
using System.IO;
using MobileCenterApp.Data;
using MobileCenterApp.Resources;
using Android.Graphics.Drawables;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	public static class ImageHelper
	{
		public static void SaveImage (string svg, double size, string cachedPath)
		{
			var image = LoadBitmap(svg, size);
			image.Compress (Android.Graphics.Bitmap.CompressFormat.Png, 100, File.Open (cachedPath, FileMode.Create));
		}

		static Android.Graphics.Bitmap LoadBitmap (string svg, double size)
		{
			var svgStream = new StreamReader(ResourceHelper.GetEmbeddedResourceStream(svg));
			var image = svgStream.LoadImageFromSvgStream(size);
			return image;
		}

		public static string GetCachedImagedName (string svg, double size)
		{
			var name = Path.GetFileNameWithoutExtension (svg);
			var cachedName = $"{name}-{size}.png";
			var cachedImage = Path.Combine (Locations.ImageCache, cachedName);
			return cachedImage;
		}

		public static async Task<Drawable> GetCachedDrawable(string svg, double size, bool cached)
		{
			if (!cached)
			{
				var bitmap = LoadBitmap(svg, size);
				return new BitmapDrawable(bitmap);
			}
			var imagePath = GetCachedImagedName(svg, size);
			if (!File.Exists(imagePath))
				await Task.Run(()=>SaveImage(svg,size,imagePath));
			return await Drawable.CreateFromPathAsync(imagePath);
		}

	}
}

