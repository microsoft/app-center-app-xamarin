using System;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class SvgImageView : Image
	{
		public static readonly BindableProperty SvgProperty = BindableProperty.Create (nameof(Svg), typeof (string), typeof (SvgImageView), default (string));
		public string Svg {
			get { return (string)GetValue (SvgProperty); }
			set { SetValue (SvgProperty, value); }
		}

		public static readonly BindableProperty CacheImagesProperty = BindableProperty.Create (nameof (CacheImages), typeof (bool), typeof (SvgImageView), default (bool));
		public bool CacheImages {
			get { return (bool)GetValue (CacheImagesProperty); }
			set { SetValue (CacheImagesProperty, value); }
		}

	}
}

