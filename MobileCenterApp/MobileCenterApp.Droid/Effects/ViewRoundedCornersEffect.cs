using System;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MobileCenterApp")]
[assembly: ExportEffect(typeof(MobileCenterApp.Droid.ViewRoundedCornersEffect), "ViewRoundedCornersEffect")]
namespace MobileCenterApp.Droid
{
    public class ViewRoundedCornersEffect : PlatformEffect
    {
        private GradientDrawable gradientDrawable;

        protected override void OnAttached()
        {
            try
            {
                gradientDrawable = new GradientDrawable();
                gradientDrawable.SetShape(ShapeType.Rectangle);
                SetBackgroundColorAndStroke();
                gradientDrawable.SetCornerRadius(DpToPixels(Android.App.Application.Context, RoundedCornersEffect.GetCornerRadius(Element)));
                Control.SetBackground(gradientDrawable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: {0}", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            //clean up
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == "BackgroundColor")
            {
                SetBackgroundColorAndStroke();
            }
        }

        private void SetBackgroundColorAndStroke()
        {
            var bkColor = RoundedCornersEffect.GetBackgroundColor(Element).ToAndroid();
            gradientDrawable.SetColor(bkColor);
            gradientDrawable.SetStroke(1, bkColor);    //required. Could be customized if PCL class RoundedCornersEffect is expanded
        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }

    }
}