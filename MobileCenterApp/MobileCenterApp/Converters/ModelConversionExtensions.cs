using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

		public static CommitClass ToCommit(this MobileCenterApi.Commit c, string appId)
		{
			return new CommitClass
			{
				AppId = appId,
				Sha = c.Sha,
				Url = c.Url,
				Message = c.Message,
			};
		}

		public static Build ToBuild(this MobileCenterApi.Build b, string appId)
		{
			return new Build
			{
				BuildId = b.Id,
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

		public static CommitClass ToCommit(this MobileCenterApi.CommitsResult c, string appId)
		{
			return new CommitClass
			{
				AppId = appId,
				Sha = c.Sha,
				Url = c.Commit.Url,
				Message = c.Commit.Message,
			};
		}

		public static List<LogSection> ToLogSections(this MobileCenterApi.LogResponse response)
		{
			int i = 0;
			var logs = ParseLogSection(ref i, response.Logs);
			return logs;
		}

		static List<LogSection> ParseLogSection(ref int i, string[] lines, int depth = 0)
		{
			var startingDepth = depth;
			var sections = new List<LogSection>();
			LogSection currentSection = null;
			while (i < lines.Length)
			{
				var line = lines[i];

				var parts = line.Split(new[] { ' ' }, 2);
				string date = parts[0];
				string message = parts[1];

				if (!message.StartsWith("##[section]"))
				{
					if (currentSection == null)
						return sections;
					currentSection.Lines.Add(message);
					i++;
					continue;
				}
				var isStart = message.Contains("Start:") || message.Contains("Starting:");
				if (isStart)
				{
					depth++;
					if (currentSection != null)
					{
						currentSection.Sections.AddRange(ParseLogSection(ref i, lines, depth));
					}
					else
					{
						message = message.Substring(message.IndexOf(':') + 1).Trim();
						sections.Add(currentSection = new LogSection
						{
							Message = message,
						});
						i++;
					}
				}
				else
				{
					depth--;
					if (depth < startingDepth)
						return sections;

					i++;
					currentSection = null;
				}

			}

			return sections;

		}

		public static Release ToRelease(this MobileCenterApi.ReleaseDetails releaseDetails, AppClass app)
		{
			return new Release
			{
				AppIconUrl = releaseDetails.AppIconUrl,
				AppId = app.Id,
				AppName = releaseDetails.AppName,
				DownloadUrl = releaseDetails.DownloadUrl,
				Fingerprint = releaseDetails.Fingerprint,
				Id = releaseDetails.Id,
				InstallUrl = releaseDetails.InstallUrl,
				MinOs = releaseDetails.MinOs,
				ProvisioningProfileName = releaseDetails.ProvisioningProfileName,
				ReleaseNotes = releaseDetails.ReleaseNotes,
				ShortVersion = releaseDetails.ShortVersion,
				Size = releaseDetails.Size,
				Status = releaseDetails.Status,
				UploadedAt = releaseDetails.UploadedAt,
				Version = releaseDetails.Version,
			};
		}
	}
}