using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MobileCenterApp
{
	public class SyncManager
	{
		public static SyncManager Shared { get; set; } = new SyncManager();
		public MobileCenterApi.MobileCenterAPIServiceApiKeyApi Api { get; set; } = new MobileCenterApi.MobileCenterAPIServiceApiKeyApi("MobileCenter", "Ttw8AMUjYeEkr==");

		Task syncAppsTask;
		public Task SyncApps()
		{
			if (syncAppsTask?.IsCompleted ?? true)
				syncAppsTask = syncApps();
			return syncAppsTask;
		}

		async Task syncApps()
		{
			if (Settings.IsOfflineMode)
				return;
			var apps = await Api.GetApps();
			List<Owner> owners = new List<Owner>();
			List<AppClass> myApps = new List<AppClass>();

			apps.ToList().ForEach(x =>
			{
				myApps.Add(new AppClass
				{
					Id = x.Id,
					AppSecret = x.AppSecret,
					AzureSubscriptionId = x.AzureSubscriptionId,
					DateImported = DateTime.Now,
					Description = x.Description,
					DisplayName = x.DisplayName,
					IndexCharacter = BaseModel.GetIndexChar(x.DisplayName),
					IconUrl = x.IconUrl,
					Name = x.Name,
					Os = x.Os,
					OwnerId = x.Owner.Id,
					Platform = x.Platform,
				});
				var o = x.Owner;
				owners.Add(new Owner
				{
					AvatarUrl = o.AvatarUrl,
					DisplayName = o.DisplayName,
					Email = o.Email,
					Id = o.Id,
					Name = o.Name,
					Type = o.Type,
				});
			});

			await Database.Main.ResetTable<AppClass>();
			await Database.Main.InsertAllAsync(myApps);
			var distintOwners = owners.DistinctBy(x => x.Id).ToList();
			Database.Main.InsertOrReplaceAll(distintOwners);
			NotificationManager.Shared.ProcAppsChanged();
		}
	}
}
