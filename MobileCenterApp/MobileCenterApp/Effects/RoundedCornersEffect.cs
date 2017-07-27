using System.Linq;
using Xamarin.Forms;

namespace MobileCenterApp
{
    public static class RoundedCornersEffect
    {
        public static readonly BindableProperty HasRoundedCornersProperty = BindableProperty.CreateAttached("HasRoundedCorners", typeof(bool), typeof(RoundedCornersEffect), false, propertyChanged: OnHasRoundedCornersChanged);
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.CreateAttached("BackgroundColor", typeof(Color), typeof(RoundedCornersEffect), Color.Default);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached("CornerRadius", typeof(float), typeof(RoundedCornersEffect), 0.0f);

        public static bool GetHasRoundedCorners(BindableObject view)
        {
            return (bool)view.GetValue(HasRoundedCornersProperty);
        }

        public static void SetHasRoundedCorners(BindableObject view, bool value)
        {
            view.SetValue(HasRoundedCornersProperty, value);
        }

        public static Color GetBackgroundColor(BindableObject view)
        {
            return (Color)view.GetValue(BackgroundColorProperty);
        }

        public static void SetBackgroundColor(BindableObject view, Color value)
        {
            view.SetValue(BackgroundColorProperty, value);
        }

        public static float GetCornerRadius(BindableObject view)
        {
            return (float)view.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(BindableObject view, float value)
        {
            view.SetValue(CornerRadiusProperty, value);
        }

        static void OnHasRoundedCornersChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
            {
                return;
            }

            var hasRoundedCorners = (bool)newValue;

            if (hasRoundedCorners)
            {
                view.Effects.Add(new ViewRoundedCornersEffect());
            }
            else
            {
                var effect = view.Effects.FirstOrDefault(q => q is ViewRoundedCornersEffect);
                if (effect != null)
                {
                    view.Effects.Remove(effect);
                }
            }
        }

        class ViewRoundedCornersEffect : RoutingEffect
        {
            public ViewRoundedCornersEffect() : base("MobileCenterApp.ViewRoundedCornersEffect")
            {
            }
        }
    }
}