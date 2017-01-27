using System;
using System.IO;
using NGraphics;
using Android.Widget;
using Android.Graphics;

namespace MobileCenterApp
{
	public static class NGraphicsExtensions
	{
		static readonly IPlatform Platform = new AndroidPlatform ();
		static readonly double Scale = (double)Xamarin.Forms.Forms.Context.Resources.DisplayMetrics.Density;

		public static void LoadSvg (this ImageView imageView, string svg, Size size)
		{
			var image = svg.LoadImageFromSvg (size);
			imageView.SetImageBitmap (image);
		}
		public static Bitmap LoadImageFromSvg (this string svg, Double size)
		{
			return svg.LoadImageFromSvg (new Size (size, size));
		}

		public static Bitmap LoadImageFromSvg (this string svg, Size size)
		{
			try {
				var fileName = System.IO.Path.GetFileNameWithoutExtension (svg);
				var file = File.OpenText (fileName);
				return file.LoadImageFromSvgStream (size);
			} catch (Exception ex) {
				Console.WriteLine (ex);
				Console.WriteLine ("Failed parsing: {0}", svg);
				throw;
			}
		}
		public static Bitmap LoadImageFromSvgStream (this TextReader file, double size)
		{
			return file.LoadImageFromSvgStream (new Size (size, size));
		}
		public static Bitmap LoadImageFromSvgStream (this TextReader file, Size size)
		{

			var graphic = Graphic.LoadSvg (file);
			//Shame on Size not being Equatable ;)
			if (size.Width <= 0 || size.Height <= 0)
				size = graphic.Size;
			var gSize = graphic.Size;
			if (gSize.Width > size.Width || size.Height > gSize.Height) {
				var ratioX = size.Width / gSize.Width;
				var ratioY = size.Height / gSize.Height;
				var ratio = Math.Min (ratioY, ratioX);
				graphic.Size = size = new Size (gSize.Width * ratio, gSize.Height * ratio);
			}
			var c = Platform.CreateImageCanvas (size, Scale);
			graphic.Draw (c);
			var image = c.GetImage () as BitmapImage;
			return image.Bitmap;


		}
	}
}