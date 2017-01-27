using System;
using Xamarin.Forms.Platform.iOS;
using MobileCenterApp.Resources;
using Xamarin.Forms;
using MobileCenterApp;
using MobileCenterApp.iOS;

[assembly: ExportRenderer (typeof (SvgImageView), typeof (SvgImageViewRenderer))]
namespace MobileCenterApp.iOS
{
	public class SvgImageViewRenderer : ImageRenderer
	{
		public SvgImageViewRenderer ()
		{
			
		}
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == nameof (SvgImageView.Svg))
				LayoutSubviews ();
		}
		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
			this.LayoutSubviews ();
		}

		CoreGraphics.CGRect lastRect;
		public override void LayoutSubviews ()
		{
			try
			{
				base.LayoutSubviews();
				LoadImage();
			}
			catch (Exception)
			{
				//Layout has been causing a null object exception :(
			}
		}
		async void LoadImage()
		{
			var svgImage = (SvgImageView)Element;
			if (lastRect == Bounds || Control == null || string.IsNullOrWhiteSpace(svgImage?.Svg))
				return;
			lastRect = Bounds;

			var width = Bounds.Width;
			var height = Bounds.Height;
			bool cached = svgImage.CacheImages;

			var image = await ImageHelper.GetImage(svgImage.Svg, Math.Max(width, height), cached);
			Control.Image = image;
		}
	}
}

