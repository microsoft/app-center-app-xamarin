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

		public static Branch ToBranch(this MobileCenterApi.BranchStatus b, string appId)
		{
			return new Branch
			{
				AppId = appId,
				Name = b.Branch.Name,
				LastBuildId = b.LastBuild?.Id ?? 0,
				IndexCharacter = BaseModel.GetIndexChar(b.Branch.Name),
				LastCommitId = b.Branch.Commit?.Sha,
				BuildStatus = b?.LastBuild?.Status ?? "Never Built",
			};
		}

		public static Commit ToCommit(this MobileCenterApi.Commit c, string appId)
		{
			return new Commit
			{
				AppId = appId,
				Sha = c.Sha,
				Url = c.Url,
			};
		}

		public static Build ToBuild(this MobileCenterApi.Build b, string appId)
		{
			return new Build
			{
				Id = b.Id,
				AppId = appId,
				BuildNumber = b.BuildNumber,
				FinishTime = b.FinishTime,
				LastChangedDate = b.LastChangedDate,
				QueueTime = b.QueueTime,
				Result = b.Result,
				SourceBranch = b.SourceBranch,
				SourceVersion = b.SourceVersion,
				StartTime = b.StartTime,
				Status = b.Status,
			};
		}

		public static RepoConfig ToRepoConfig(this MobileCenterApi.RepoConfig rc, string appId)
		{
			return new RepoConfig
			{
				AppId = appId,
				Id = rc.Id,
				RepoUrl = rc.RepoUrl,
				State = rc.State,
				Type = rc.Type,
			};
		}
	}
}
