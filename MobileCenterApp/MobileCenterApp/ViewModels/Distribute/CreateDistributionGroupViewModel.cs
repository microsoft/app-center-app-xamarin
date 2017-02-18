using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class CreateDistributionGroupViewModel : BaseViewModel
	{
		public CreateDistributionGroupViewModel()
		{
			Title = "Create Distribution Group";
		}
		public ICommand CancelCommand { get; private set; } = new Command(async (obj) =>
		{
			await NavigationService.PopModalAsync();
		});
	}
}
