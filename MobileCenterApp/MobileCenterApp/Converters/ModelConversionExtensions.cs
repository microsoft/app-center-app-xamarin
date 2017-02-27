using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MobileCenterApp
{
	public static class ModelConversionExtensions
	{
		public static T ToEnum<T>(this string str) where T : struct, IConvertible
		{
			return (T)Enum.Parse(typeof(T), str,true);
		}

		public static AppClass ToAppClass(this MobileCenterApi.Models.AppResponse app)
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
				Os = app.Os.ToString(),
				OwnerId = app.Owner.Id,
				Platform = app.Platform.ToString(),
			};
		}

		public static MobileCenterApi.Models.AppRequest ToAppRequest(this AppClass app)
		{
			return new MobileCenterApi.Models.AppRequest
			{
				Description = app.Description?.Trim(),
				DisplayName = app.DisplayName?.Trim(),
				Name = app.Name?.Trim(),
				Os = app.Os.ToEnum<MobileCenterApi.Models.AppRequestOs>(),
				Platform = app.Platform.ToEnum<MobileCenterApi.Models.AppRequestPlatform>(),
			};
		}

		public static Owner ToAppOwner(this MobileCenterApi.Models.Owner o)
		{
			return new Owner
			{
				AvatarUrl = o.AvatarUrl,
				DisplayName = o.DisplayName,
				Email = o.Email,
				Id = o.Id,
				Name = o.Name,
				Type = o.Type.ToString(),
			};
		}

		public static Branch ToBranch(this MobileCenterApi.Models.BranchStatus b, string appId)
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

		public static CommitClass ToCommit(this MobileCenterApi.Models.Commit c, string appId)
		{
			return new CommitClass
			{
				AppId = appId,
				Sha = c.Sha,
				Url = c.Url,
				Message = c.Message,
			};
		}

		public static Build ToBuild(this MobileCenterApi.Models.Build b, string appId)
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

		public static RepoConfig ToRepoConfig(this MobileCenterApi.Models.RepoConfig rc, string appId)
		{
			return new RepoConfig
			{
				AppId = appId,
				Id = rc.Id,
				RepoUrl = rc.RepoUrl,
				State = rc.State.ToString(),
				Type = rc.Type,
			};
		}

		public static CommitClass ToCommit(this MobileCenterApi.Models.CommitsResult c, string appId)
		{
			return new CommitClass
			{
				AppId = appId,
				Sha = c.Sha,
				Url = c.Commit.Url,
				Message = c.Commit.Message,
			};
		}

		public static List<LogSection> ToLogSections(this MobileCenterApi.Models.LogResponse response)
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

		public static Release ToRelease(this MobileCenterApi.Models.ReleaseDetails releaseDetails, AppClass app)
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
				Status = releaseDetails.Status.ToString(),
				UploadedAt = releaseDetails.UploadedAt,
				Version = releaseDetails.Version,
			};
		}

		public static Release UpdateRelease(this MobileCenterApi.Models.ReleaseDetails releaseDetails, Release release)
		{
			release.AppIconUrl = releaseDetails.AppIconUrl;
			release.AppName = releaseDetails.AppName;
			release.DownloadUrl = releaseDetails.DownloadUrl;
			release.Fingerprint = releaseDetails.Fingerprint;
			release.Id = releaseDetails.Id;
			release.InstallUrl = releaseDetails.InstallUrl;
			release.MinOs = releaseDetails.MinOs;
			release.ProvisioningProfileName = releaseDetails.ProvisioningProfileName;
			release.ReleaseNotes = releaseDetails.ReleaseNotes;
			release.ShortVersion = releaseDetails.ShortVersion;
			release.Size = releaseDetails.Size;
			release.Status = releaseDetails.Status.ToString();
			release.UploadedAt = releaseDetails.UploadedAt;
			release.Version = releaseDetails.Version;
			return release;
		}

		public static DistributionGroup ToDistributionGroup(this MobileCenterApi.Models.DistributionGroupResponse request, AppClass app)
		{
			return new DistributionGroup
			{
				AppId = app.Id,
				Name = request.Name,
				Id = request.Id,
				IndexCharacter = BaseModel.GetIndexChar(request.Name),
			};
		}
		public static User ToUser(this MobileCenterApi.Models.DistributionGroupUserGetResponse resp)
		{
			return new User
			{
				AvatarUrl = resp.AvatarUrl,
				Id = resp.Id,
				CanChangePassword = resp.CanChangePassword,
				DisplayName = resp.DisplayName,
				Email = resp.Email,
				Name = resp.Name,
				IndexCharacter = BaseModel.GetIndexChar(resp.DisplayName),
			};
		}
		public static Tester ToTester(this MobileCenterApi.Models.DistributionGroupUserGetResponse resp, DistributionGroup distribution, User user)
		{
			return new Tester
			{
				DisplayName = user.DisplayName,
				DistributionId = distribution.Id,
				User = user,
				InvitePending = resp.InvitePending,
				AppId = distribution.AppId,
			};
		}

		public static CrashGroup ToCrashGroup(this MobileCenterApi.Models.CrashGroup crash, AppClass app)
		{
			return new CrashGroup
			{
				AppId = app.Id,
				AppVersion = crash.AppVersion,
				Build = crash.Build,
				Count = crash.Count,
				DisplayId = crash.DisplayId,
				ErrorReason = crash.ErrorReason,
				Exception = crash.Exception,
				Fatal = crash.Fatal,
				FirstOccurrence = crash.FirstOccurrence,
				Id = crash.CrashGroupId,
				ImpactedUsers = crash.ImpactedUsers,
				LastOccurrence = crash.LastOccurrence,
				Status = (CrashGroupStatus)(int)crash.Status
			};
		}

		public static ReasonStackFrame ToReasonStackFrame(this MobileCenterApi.Models.ReasonStackFrame stack, CrashGroup crashGroup)
		{
			return new ReasonStackFrame
			{
				AppCode = stack.AppCode,
				AppId = crashGroup.AppId,
				ClassMethod = stack.ClassMethod,
				ClassName = stack.ClassName,
				CodeFormatted = stack.CodeFormatted,
				CrashGroupId = crashGroup.Id,
				File = stack.File,
				FrameworkName = stack.FrameworkName,
				Language = stack.Language.HasValue ? (ReasonStackFrameLanguage?)stack.Language : null,
				Line = stack.Line,
				Method = stack.Method,
				MethodParams = stack.MethodParams,
			};
		}
	}
}