using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MobileCenterApp.Data;
using System.IO;
using SimpleAuth;
namespace MobileCenterApp
{
	public class SyncManager
	{
		public static SyncManager Shared { get; set; } = new SyncManager();
		public MobileCenterApi.MobileCenterAPIServiceApiKeyApi Api { get; set; } = new MobileCenterApi.MobileCenterAPIServiceApiKeyApi("MobileCenter", "Ttw8AMUjYeEkr=="
#if __MOBILE__
																																	   , new ModernHttpClient.NativeMessageHandler()
#endif
																																	  );
		public SyncManager()
		{
#if DEBUG
			Api.Verbose = true;
#endif
		}

		#region Account
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
			var apps = await Api.Account.GetApps1();
			List<Owner> owners = new List<Owner>();
			List<AppClass> myApps = new List<AppClass>();

			apps.ToList().ForEach(x =>
			{
				myApps.Add((AppClass)x.ToAppClass());
				owners.Add((Owner)x.Owner.ToAppOwner());
			});

			await Database.Main.ResetTable<AppClass>();
			await Database.Main.InsertAllAsync(myApps);
			var distintOwners = owners.DistinctBy(x => x.Id).ToList();
			Database.Main.InsertOrReplaceAll(distintOwners);
			NotificationManager.Shared.ProcAppsChanged();
		}

		Task<User> userTask;
		public Task<User> GetUser()
		{
			if (userTask?.IsCompleted ?? true)
				userTask = Task.Run(async () =>
				{
					var profile = await Api.Account.GetUserProfile();
					var user = new User
					{
						AvatarUrl = profile.AvatarUrl,
						DisplayName = profile.DisplayName,
						CanChangePassword = profile.CanChangePassword,
						Email = profile.Email,
						Id = profile.Id,
						Name = profile.Name,
					};
					Settings.CurrentUser = user;
					return user;
				});
			return userTask;
		}

		public async Task<bool> CreateApp(AppClass app)
		{
			try
			{
				var resp = await Api.Account.CreateApp(app.ToAppRequest());
				var newApp = resp.ToAppClass();
				var owner = resp.Owner.ToAppOwner();
				Database.Main.InsertOrReplace(newApp);
				Database.Main.InsertOrReplace(owner);
				NotificationManager.Shared.ProcAppsChanged();
				return true;
			}
			catch (Exception ex)
			{
				if (ex.Data.Contains("HttpContent"))
				{
					Debug.WriteLine(ex.Data["HttpContent"]);
				}
				Debug.WriteLine(ex);
			}
			return false;
		}

		public async Task<bool> DeleteApp(AppClass app)
		{
			try
			{
				await Api.Account.DeleteApp(app.Name, app.Owner.Name);
				Database.Main.Delete(app);
				NotificationManager.Shared.ProcAppsChanged();
				return true;
			}
			catch (Exception ex)
			{
				if (ex.Data.Contains("HttpContent"))
				{
					Debug.WriteLine(ex.Data["HttpContent"]);
				}
				else
					Debug.WriteLine(ex);
			}
			return false;
		}
		#endregion //Account

		#region Build
		Dictionary<string, Task> syncbranchesTask = new Dictionary<string, Task>();
		public Task SyncBranch(AppClass app)
		{
			Task syncBranchTask;
			syncbranchesTask.TryGetValue(app.Id, out syncBranchTask);
			if (syncBranchTask?.IsCompleted ?? true)
				syncbranchesTask[app.Id] = syncBranchTask = syncBranch(app);
			return syncBranchTask;
		}

		async Task syncBranch(AppClass app)
		{
			var branchStatus = await Api.Build.GetBranches(app.Owner.Name, app.Name);

			var branches = new List<Branch>();
			var commits = new List<CommitClass>();
			var builds = new List<Build>();
			branchStatus.ToList().ForEach(x =>
			{
				branches.Add(x.ToBranch(app.Id));
				if (x?.Branch?.Commit != null)
					commits.Add(x.Branch.Commit.ToCommit(app.Id));
				if (x?.LastBuild != null)
					builds.Add(x.LastBuild.ToBuild(app.Id));
			});
			await Database.Main.ExecuteAsync("delete from Branch where AppId = ?", app.Id);
			await Database.Main.InsertAllAsync(branches);
			var distinctCommits = commits.DistinctBy(x => x.Sha).ToList();
			Database.Main.InsertOrReplaceAll(distinctCommits);
			var distinctBuilds = builds.DistinctBy(x => x.Id).ToList();
			Database.Main.InsertOrReplaceAll(distinctBuilds);

			NotificationManager.Shared.ProcBranchesChanged(app.Id);
			syncbranchesTask.Remove(app.Id);
		}


		Dictionary<string, Task> syncRepoConfigTasks = new Dictionary<string, Task>();
		public Task SyncRepoConfig(AppClass app)
		{
			Task syncRepoConfigTask;
			syncRepoConfigTasks.TryGetValue(app.Id, out syncRepoConfigTask);
			if (syncRepoConfigTask?.IsCompleted ?? true)
				syncRepoConfigTasks[app.Id] = syncRepoConfigTask = syncRepoConfig(app);
			return syncRepoConfigTask;
		}

		async Task syncRepoConfig(AppClass app)
		{
			var configs = await Api.Build.GetRepositoryConfiguration(app.Owner.Name, app.Name, true).ConfigureAwait(false);
			Database.Main.InsertOrReplaceAll(configs.Select(x => x.ToRepoConfig(app.Id)));
			syncRepoConfigTasks.Remove(app.Id);
		}


		Dictionary<string, Task> syncBuildsTasks = new Dictionary<string, Task>();
		public Task SyncBuilds(Branch branch)
		{
			Task syncBuildsTask;
			syncBuildsTasks.TryGetValue(branch.Id, out syncBuildsTask);
			if (syncBuildsTask?.IsCompleted ?? true)
				syncBuildsTasks[branch.Id] = syncBuildsTask = syncBuilds(branch);
			return syncBuildsTask;
		}

		async Task syncBuilds(Branch branch)
		{
			var app = branch.App;
			var builds = await Api.Build.GetBranchBuilds(branch.Name, app.Owner.Name, app.Name).ConfigureAwait(false);
			var myBuilds = builds.Select(x => x.ToBuild(app.Id)).ToList();
			await Database.Main.ExecuteAsync("delete from Build where AppId = ? and SourceBranch = ?", app.Id, branch.Name);

			Database.Main.InsertOrReplaceAll(myBuilds);
			//Hopefully we can remove this nonsense later.
			var missingShas = myBuilds.Where(x => x.LastCommit == null || string.IsNullOrWhiteSpace(x.LastCommit.Message)).Select(x => x.SourceVersion).ToList();
			if (missingShas.Any())
			{
				var shaString = string.Join(",", missingShas);
				var commits = await Api.GetCommits(shaString, app.Owner.Name, app.Name, "full");
				if (commits.Any())
				{
					Database.Main.InsertOrReplaceAll(commits.Select(x => x.ToCommit(app.Id)));
				}
			}
			NotificationManager.Shared.ProcBuildsChanged(app.Id);
			syncBuildsTasks.Remove(branch.Id);
		}

		Dictionary<string, Task<List<LogSection>>> downloadLogsTask = new Dictionary<string, Task<List<LogSection>>>();
		public Task<List<LogSection>> DownloadLog(Build build)
		{
			Task<List<LogSection>> syncBuildsTask;
			downloadLogsTask.TryGetValue(build.Id, out syncBuildsTask);
			if (syncBuildsTask?.IsCompleted ?? true)
				downloadLogsTask[build.Id] = syncBuildsTask = downloadLog(build);
			return syncBuildsTask;
		}

		async Task<List<LogSection>> downloadLog(Build build)
		{
			var tempPath = Path.Combine(Locations.TempDir, $"{build.Id}.log");
			if (File.Exists(tempPath))
			{
				return await Task.Run(() =>
				{
					var json = File.ReadAllText(tempPath);
					return json.ToObject<List<LogSection>>();
				});
			}

			var app = Database.Main.GetObject<AppClass>(build.AppId);
			var logData = await Api.Build.GetBuildLogs(build.BuildId, app.Owner.Name, app.Name).ConfigureAwait(false);
			var logs = logData.ToLogSections();
			//Write our temp data
			File.WriteAllText(tempPath, logs.ToJson());
			downloadLogsTask.Remove(build.Id);
			return logs;

		}
		#endregion //Build

		#region Distribution

		Dictionary<string, Task> distributionReleaseTaskTask = new Dictionary<string, Task>();
		public Task SyncReleases(AppClass app)
		{
			Task syncReleasesTask;
			distributionReleaseTaskTask.TryGetValue(app.Id, out syncReleasesTask);
			if (syncReleasesTask?.IsCompleted ?? true)
				distributionReleaseTaskTask[app.Id] = syncReleasesTask = syncReleases(app);
			return syncReleasesTask;
		}

		async Task syncReleases(AppClass app)
		{
			var releases = await Api.Distribute.GetV01AppsReleases(app.Owner.Name, app.Name).ConfigureAwait(false);
			//This one does ignore. Just incase we fixed the missing fields
			Database.Main.InsertOrIgnoreAll(releases.Select(x => x.ToRelease(app)));
			NotificationManager.Shared.ProcReleasesChanged(app.Id);
			distributionReleaseTaskTask.Remove(app.Id);
		}



		Dictionary<string, Task> distributionReleaseDetailsTasks = new Dictionary<string, Task>();
		public Task SyncReleasesDetails(Release release)
		{
			Task syncReleasesTask;
			distributionReleaseDetailsTasks.TryGetValue(release.ReleaseId, out syncReleasesTask);
			if (syncReleasesTask?.IsCompleted ?? true)
				distributionReleaseDetailsTasks[release.ReleaseId] = syncReleasesTask = syncReleasesDetails(release);
			return syncReleasesTask;
		}

		async Task syncReleasesDetails(Release release)
		{
			var app = Database.Main.GetObject<AppClass>(release.AppId);
			var r = await Api.Distribute.GetReleaseOrLatestRelease(release.Id, app.Owner.Name, app.Name).ConfigureAwait(false);
			Database.Main.InsertOrReplace(r.ToRelease(app));
			NotificationManager.Shared.ProcReleaseDetailsChanged(r.Id);
			distributionReleaseDetailsTasks.Remove(release.ReleaseId);
		}

		Dictionary<string, Task> syncDistributionGroupsTasks = new Dictionary<string, Task>();
		public Task SyncDistributionGroups(AppClass app)
		{
			Task syncReleasesTask;
			syncDistributionGroupsTasks.TryGetValue(app.Id, out syncReleasesTask);
			if (syncReleasesTask?.IsCompleted ?? true)
				syncDistributionGroupsTasks[app.Id] = syncReleasesTask = syncDistributionGroups(app);
			return syncReleasesTask;
		}

		async Task syncDistributionGroups(AppClass app)
		{
			var groups = await Api.Account.GetDistributionGroups(app.Owner.Name, app.Name);
			Database.Main.InsertOrReplaceAll(groups.Select(x => x.ToDistributionGroup(app)));
			NotificationManager.Shared.ProcDistributionGroupsChanged(app.Id);
			syncDistributionGroupsTasks.Remove(app.Id);
		}


		Dictionary<string, Task<bool>> deleteDistributionGroupTasks = new Dictionary<string, Task<bool>>();
		public Task<bool> Delete(DistributionGroup distribution)
		{
			Task<bool> deleteDistributionGroupTask;
			deleteDistributionGroupTasks.TryGetValue(distribution.Id, out deleteDistributionGroupTask);
			if (deleteDistributionGroupTask?.IsCompleted ?? true)
			{
				createDistributionGroupTasks[distribution.Id] = deleteDistributionGroupTask = delete(distribution);
			}
			return deleteDistributionGroupTask;
		}

		async Task<bool> delete(DistributionGroup distribution)
		{
			try
			{
				var app = Database.Main.GetObject<AppClass>(distribution.AppId);
				await Api.Account.DeleteDistributionGroup(app.Name, app.Owner.Name, distribution.Name);
				Database.Main.Delete(distribution);
				NotificationManager.Shared.ProcDistributionGroupsChanged(app.Id);
				return true;
			}
			catch (Exception ex)
			{
				LogManager.Shared.Report(ex);
				return false;
			}
			finally
			{

				deleteDistributionGroupTasks.Remove(distribution.Id);
			}
		}


		Dictionary<string, Task<bool>> createDistributionGroupTasks = new Dictionary<string, Task<bool>>();
		public Task<bool> CreateDistributionGroup(AppClass app, string name)
		{

			Task<bool> createDistributionGroupTask;
			createDistributionGroupTasks.TryGetValue(name, out createDistributionGroupTask);
			if (createDistributionGroupTask?.IsCompleted ?? true)
			{
				createDistributionGroupTasks[name] = createDistributionGroupTask = createDistributionGroup(app, name);
			}
			return createDistributionGroupTask;

		}

		async Task<bool> createDistributionGroup(AppClass app, string name)
		{
			try
			{
				var distribution = await Api.Account.CreateDistributionGroup(app.Owner.Name, app.Name, new MobileCenterApi.Models.DistributionGroupRequest { Name = name });
				Database.Main.Insert(distribution.ToDistributionGroup(app));
				NotificationManager.Shared.ProcDistributionGroupsChanged(app.Id);
				return true;
			}
			catch (Exception ex)
			{
				LogManager.Shared.Report(ex);
				return false;
			}
			finally
			{
				createDistributionGroupTasks.Remove(name);
			}
		}

		Dictionary<string, Task> syncDistributionGroupsMembersTasks = new Dictionary<string, Task>();
		public Task SyncDistributionGroupMembers(DistributionGroup distributionGroup)
		{
			Task syncReleasesTask;
			syncDistributionGroupsMembersTasks.TryGetValue(distributionGroup.Id, out syncReleasesTask);
			if (syncReleasesTask?.IsCompleted ?? true)
				syncDistributionGroupsMembersTasks[distributionGroup.Id] = syncReleasesTask = syncDistributionGroupMembers(distributionGroup);
			return syncReleasesTask;
		}

		async Task syncDistributionGroupMembers(DistributionGroup distributionGroup)
		{
			var app = Database.Main.GetObject<AppClass>(distributionGroup.AppId);
			var members = await Api.Account.GetDistributionGroupUsers(app.Owner.Name, app.Name, distributionGroup.Name);
			Database.Main.InsertOrReplaceAll(members.Select(x => x.ToTester(distributionGroup)));
			NotificationManager.Shared.ProcDistributionGroupMembersChanged(distributionGroup.Id);
			syncDistributionGroupsMembersTasks.Remove(distributionGroup.Id);
		}


		Dictionary<string, Task> syncDistributionGroupsReleasesTasks = new Dictionary<string, Task>();
		public Task SyncDistributionGroupReleases(DistributionGroup distributionGroup)
		{
			Task syncReleasesTask;
			syncDistributionGroupsReleasesTasks.TryGetValue(distributionGroup.Id, out syncReleasesTask);
			if (syncReleasesTask?.IsCompleted ?? true)
				syncDistributionGroupsReleasesTasks[distributionGroup.Id] = syncReleasesTask = syncDistributionGroupReleases(distributionGroup);
			return syncReleasesTask;
		}

		async Task syncDistributionGroupReleases(DistributionGroup distributionGroup)
		{
			var app = Database.Main.GetObject<AppClass>(distributionGroup.AppId);
			var resp = await Api.Distribute.GetReleasesForDistributionGroup(distributionGroup.Name, app.Owner.Name, app.Name);
			var releases = resp.Select(x => x.ToRelease(app)).ToList();
			var releaseGroups = releases.Select(x => new DistributionReleaseGroup { DistributionId = distributionGroup.Id, Release = x });
			Database.Main.InsertOrReplaceAll(releases);
			//Remove all first to handle deletes
			await Database.Main.ExecuteAsync("delete from DistributionReleaseGroup where DistributionId = ?", distributionGroup.Id);
			Database.Main.InsertOrReplaceAll(releaseGroups);
			NotificationManager.Shared.ProcDistributionGroupReleasesChanged(distributionGroup.Id);
			syncDistributionGroupsReleasesTasks.Remove(distributionGroup.Id);
		}

		#endregion //Distribution

	}
}
