using System;
namespace MobileCenterApp
{
	public static class ModelConversionExtensions
	{
		public static AppClass ToAppClass(this MobileCenterApi.AppResponse app)
		{
			return new AppClass
			{
				Id = app.Id,
				AppSecret = app.AppSecret,
				AzureSubscriptionId = app.AzureSubscriptionId,
				DateImported = DateTime.Now,
				Description = app.Description,
				DisplayName = app.DisplayName,
				IndexCharacter = BaseModel.GetIndexChar(app.DisplayName),
				IconUrl = app.IconUrl,
				Name = app.Name,
				Os = app.Os,
				OwnerId = app.Owner.Id,
				Platform = app.Platform,
			};
		}

		public static MobileCenterApi.AppRequest ToAppRequest(this AppClass app)
		{
			return new MobileCenterApi.AppRequest
			{
				Description = app.Description?.Trim(),
				DisplayName = app.DisplayName?.Trim(),
				Name = app.Name?.Trim(),
				Os = app.Os?.Trim(),
				Platform = app.Platform?.Trim(),
			};
		}

		public static Owner ToAppOwner(this MobileCenterApi.Owner o)
		{
			return new Owner
			{
				AvatarUrl = o.AvatarUrl,
				DisplayName = o.DisplayName,
				Email = o.Email,
				Id = o.Id,
				Name = o.Name,
				Type = o.Type,
			};
		}
	}
}
