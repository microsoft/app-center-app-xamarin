using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class InviteMemberViewModel : BaseViewModel
	{
		public ICommand CancelCommand { get; private set; } = new Command(async (obj) =>
		{
			await NavigationService.PopModalAsync();
		});

		public ICommand InviteCommand { get; private set; }

		public InviteMemberViewModel()
		{
			Title = "Invite Testers";
			InviteCommand = new Command(async (obj) => await InviteMember());
		}

		public string Email { get; set; }
		string error;
		public string Error
		{
			get { return error; }
			set { ProcPropertyChanged(ref error, value); }
		}

		public string DistrbutionGroupId { get; set; }

		public DistributionGroup DistributionGroup
		{
			get { return Database.Main.GetObject<DistributionGroup>(DistrbutionGroupId); }
			set { DistrbutionGroupId = value?.Id; }
		}


		public async Task InviteMember()
		{
			if (string.IsNullOrWhiteSpace(Email))
			{
				Error = "Invalid Email";
				return;
			}
			try
			{
				Error = "";
				IsLoading = true;
				var success = await SyncManager.Shared.InviteDistributionGroup(DistributionGroup, Email);
				if (success)
				{
					await NavigationService.PopModalAsync();
				}
			}
			catch (Exception ex)
			{
				LogManager.Shared.Report(ex);
				if (ex.Data.Contains("HttpContent"))
				{
					Error = ex.Data["HttpContent"].ToString();
				}
				else
					Error = ex.Message;
			}
			finally
			{
				IsLoading = false;
			}
		}
	}
}
