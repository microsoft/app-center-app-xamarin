using System;
using System.Globalization;
using Xamarin.Forms;
namespace MobileCenterApp
{
	public class ViewModelToPageConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var viewModel = value as BaseViewModel;
			return new NavigationPage(SimpleIoC.GetPage(viewModel));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
