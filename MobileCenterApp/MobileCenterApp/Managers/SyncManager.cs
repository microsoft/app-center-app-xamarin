using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MobileCenterApp.Data;
using System.IO;
using SimpleAuth;
using System.Runtime.CompilerServices;
namespace MobileCenterApp
{
	public class SyncManager
	{
		public static SyncManager Shared { get; set; } = new SyncManager();
		public MobileCenterApi.MobileCenterClientApiKeyApi Api { get; set; } = new MobileCenterApi.MobileCenterClientApiKeyApi("MobileCenter", "Ttw8AMUjYeEkr=="
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

		object taskLocker = new object();
		Dictionary<string, object> TaskDictionary = new Dictionary<string, object>();
		async Task RunSingularTask(Func<Task> getTask, string id = null, [CallerMemberName]string grouping = "")
		{
			var key = $"{grouping} - {id}";
			object obj;
			Task foundTask;
			bool shouldClear = false;
			lock (taskLocker)
			{
				TaskDictionary.TryGetValue(key, out obj);
				foundTask = obj as Task;
				if (foundTask?.IsCompleted ?? true)
				{
					TaskDictionary[key] = foundTask = getTask();
					shouldClear = true;
				}
			}
			try
			{
				await foundTask;
			}
			finally
			{
				if (shouldClear)
				{
					lock (foundTask)
					{
						TaskDictionary.Remove(key);
					}
				}
			}

		}

		async Task<T> RunSingularTask<T>(Func<Task<T>> getTask, string id = null, [CallerMemberName]string grouping = "")
		{
			var key = $"{grouping} - {id}";
			object obj;
			Task<T> foundTask;
			bool shouldClear = false;
			lock (taskLocker)
			{
				TaskDictionary.TryGetValue(key, out obj);
				foundTask = obj as Task<T>;
				if (foundTask?.IsCompleted ?? true)
				{
					TaskDictionary[key] = foundTask = getTask();
					shouldClear = true;
				}
			}
			try
			{
				return await foundTask;
			}
			finally
			{
				if (shouldClear)
				{
					lock (foundTask)
					{
						TaskDictionary.Remove(key);
					}
				}
			}
		}

		#region Account
		public Task SyncApps()
		{
			return RunSingularTask(() => syncApps());
		}

		async Task syncApps()
		{
			if (Settings.IsOfflineMode)
				return;
			var apps = await Api.Account.List4();
			var owners = new List<Owner>();
			var myApps = new List<AppClass>();

			apps.ToList().ForEach(x =>
			{
				myApps.Add(x.ToAppClass());
				owners.Add(x.Owner.ToAppOwner());
			});

			await Database.Main.ResetTable<AppClass>();
			await Database.Main.InsertAllAsync(myApps);
			var distintOwners = owners.DistinctBy(x => x.Id).ToList();
			Database.Main.InsertOrReplaceAll(distintOwners);
			NotificationManager.Shared.ProcAppsChanged();
		}

		public Task<User> GetUser()
		{
			return RunSingularTask(() => Task.Run(async () =>
				 {
					 var profile = await Api.Account.Get();
					 var user = new User
					 {
						 AvatarUrl = profile.AvatarUrl,
						 DisplayName = profile.DisplayName,
						 CanChangePassword = profile.CanChangePassword,
						 Email = profile.Email,
						 Id = profile.Id,
						 Name = profile.Name,
						 IndexCharacter = BaseModel.GetIndexChar(profile.DisplayName),
					 };
					 Settings.CurrentUser = user;
					 return user;
				 }));
		}

		public async Task<bool> CreateApp(AppClass app)
		{
			try
			{
				var resp = await Api.Account.Create3(app.ToAppRequest());
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

		public Task<bool> DeleteApp(AppClass app)
		{
			return RunSingularTask(() => deleteApp(app), app.Id);
		}
		public async Task<bool> deleteApp(AppClass app)
		{
			try
			{
				await Api.Account.Delete4(app.Name, app.Owner.Name);
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
		public Task SyncBranch(AppClass app)
		{
			return RunSingularTask(() => syncBranch(app), app.Id);
		}

		async Task syncBranch(AppClass app)
		{
			var branchStatus = await Api.Build.ListBranches(app.Owner.Name, app.Name);

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
		}

		public Task SyncRepoConfig(AppClass app)
		{
			return RunSingularTask(() => syncRepoConfig(app), app.Id);
		}

		async Task syncRepoConfig(AppClass app)
		{
			var configs = await Api.Build.List2(app.Owner.Name, app.Name, true).ConfigureAwait(false);
			Database.Main.InsertOrReplaceAll(configs.Select(x => x.ToRepoConfig(app.Id)));
		}


		public Task SyncBuilds(Branch branch)
		{
			return RunSingularTask(() => syncBuilds(branch), branch.Id);
		}

		async Task syncBuilds(Branch branch)
		{
			var app = branch.App;
			var builds = await Api.Build.ListByBranch(branch.Name, app.Owner.Name, app.Name).ConfigureAwait(false);
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
		}

		public Task<List<LogSection>> DownloadLog(Build build)
		{
			return RunSingularTask(() => downloadLog(build), build.Id);
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
			var logData = await Api.Build.GetLog(build.BuildId, app.Owner.Name, app.Name).ConfigureAwait(false);
			var logs = logData.ToLogSections();
			//Write our temp data
			File.WriteAllText(tempPath, logs.ToJson());
			return logs;

		}

		public Task QueueBranch(Branch branch)
		{
			return RunSingularTask(() => queueBranch(branch), branch.Id);
		}

		async Task queueBranch(Branch branch)
		{
			var build = await Api.Build.Queue(branch.Name, branch.App.Owner.Name, branch.App.Name);
		    //TODO: Is this correct? It returned multiple builds before: Database.Main.InsertOrReplaceAll(builds.Select(x => x.ToBuild(branch.AppId)));
		    Database.Main.InsertOrReplace(build.ToBuild(branch.AppId));
        }

		#endregion //Build

		#region Distribution
		public Task SyncReleases(AppClass app)
		{
			return RunSingularTask(() => syncReleases(app), app.Id);
		}

		async Task syncReleases(AppClass app)
		{
			var releases = await Api.Distribute.List(app.Owner.Name, app.Name).ConfigureAwait(false);
			//This one does ignore. Just incase we fixed the missing fields
			Database.Main.InsertOrIgnoreAll(releases.Select(x => x.ToRelease(app)));
			NotificationManager.Shared.ProcReleasesChanged(app.Id);
		}


		public Task SyncReleasesDetails(Release release)
		{
			return RunSingularTask(() => syncReleasesDetails(release), release.ReleaseId);
		}

		async Task syncReleasesDetails(Release release)
		{
			var app = Database.Main.GetObject<AppClass>(release.AppId);
			var r = await Api.Distribute.GetLatestByUser(release.Id, app.Owner.Name, app.Name).ConfigureAwait(false);
			Database.Main.InsertOrReplace(r.UpdateRelease(release));
		    // TODO: The Id was a string before, now a double. Are other changes needed for that? This is just a hack to get it to compile.
			NotificationManager.Shared.ProcReleaseDetailsChanged(r.Id.ToString());
		}

		public Task SyncDistributionGroups(AppClass app)
		{
			return RunSingularTask(() => syncDistributionGroups(app), app.Id);
		}

		async Task syncDistributionGroups(AppClass app)
		{
			var groups = await Api.Account.List3(app.Owner.Name, app.Name);

			Database.Main.Execute("delete from DistributionGroup where AppId = ?", app.Id);
			Database.Main.InsertOrReplaceAll(groups.Select(x => x.ToDistributionGroup(app)));
			NotificationManager.Shared.ProcDistributionGroupsChanged(app.Id);
		}

		public Task<bool> Delete(DistributionGroup distribution)
		{
			return RunSingularTask(() => delete(distribution), distribution.Id);
		}

		async Task<bool> delete(DistributionGroup distribution)
		{
			var app = Database.Main.GetObject<AppClass>(distribution.AppId);
			await Api.Account.Delete3(app.Name, app.Owner.Name, distribution.Name);
			Database.Main.Delete(distribution);
			NotificationManager.Shared.ProcDistributionGroupsChanged(app.Id);
			return true;

		}

		public Task<bool> CreateDistributionGroup(AppClass app, string name)
		{
			return RunSingularTask(() => createDistributionGroup(app, name), name);
		}

		async Task<bool> createDistributionGroup(AppClass app, string name)
		{
			var distribution = await Api.Account.Create2(app.Owner.Name, app.Name, new MobileCenterApi.Models.DistributionGroupRequest { Name = name });
			Database.Main.Insert(distribution.ToDistributionGroup(app));
			NotificationManager.Shared.ProcDistributionGroupsChanged(app.Id);
			return true;
		}

		public Task SyncDistributionGroupMembers(DistributionGroup distributionGroup)
		{
			return RunSingularTask(() => syncDistributionGroupMembers(distributionGroup), distributionGroup.Id);
		}

		async Task syncDistributionGroupMembers(DistributionGroup distributionGroup)
		{
			var app = Database.Main.GetObject<AppClass>(distributionGroup.AppId);
			var members = await Api.Account.ListUsers(app.Owner.Name, app.Name, distributionGroup.Name);
			var users = new List<User>();
			var testers = new List<Tester>();
			foreach (var resp in members)
			{
				var user = resp.ToUser();
				testers.Add(resp.ToTester(distributionGroup, user));
				users.Add(user);
			}
			Database.Main.InsertOrReplaceAll(users);
			//Handle deletes
			await Database.Main.ExecuteAsync("delete from Tester where DistributionId = ? and AppId = ?", distributionGroup.Id, distributionGroup.AppId);
			Database.Main.InsertOrReplaceAll(testers);
			NotificationManager.Shared.ProcDistributionGroupMembersChanged(distributionGroup.Id);
		}

		public Task SyncDistributionGroupReleases(DistributionGroup distributionGroup)
		{
			return RunSingularTask(() => syncDistributionGroupReleases(distributionGroup), distributionGroup.Id);
		}

		async Task syncDistributionGroupReleases(DistributionGroup distributionGroup)
		{
			var app = Database.Main.GetObject<AppClass>(distributionGroup.AppId);
			var resp = await Api.Distribute.ListByDistributionGroup(distributionGroup.Name, app.Owner.Name, app.Name);
			var releases = resp.Select(x => x.ToRelease(app)).ToList();
			var releaseGroups = releases.Select(x => new DistributionReleaseGroup { DistributionId = distributionGroup.Id, Release = x });
			Database.Main.Execute("delete from Release where AppId = ? and ReleaseId in (select ReleaseId from DistributionReleaseGroup where DistributionId = ?)", distributionGroup.AppId, distributionGroup.Id);
			Database.Main.InsertOrReplaceAll(releases);
			//Remove all first to handle deletes
			await Database.Main.ExecuteAsync("delete from DistributionReleaseGroup where DistributionId = ?", distributionGroup.Id);
			Database.Main.InsertOrReplaceAll(releaseGroups);
			NotificationManager.Shared.ProcDistributionGroupReleasesChanged(distributionGroup.Id);
		}

		public Task<bool> InviteDistributionGroup(DistributionGroup distributionGroup, string email)
		{
			return RunSingularTask(() => inviteDistributionGroup(distributionGroup, email), email);
		}

		async Task<bool> inviteDistributionGroup(DistributionGroup distribution, string email)
		{
			var app = Database.Main.GetObject<AppClass>(distribution.AppId);
			var response = (await Api.Account.AddUser(app.Owner.Name, app.Name, distribution.Name, new MobileCenterApi.Models.DistributionGroupUserRequest { UserEmails = new string[] { email } })).First();
			return true;
		}

		public Task<bool> RemoveTester(Tester tester)
		{
			return RunSingularTask(() => removeTester(tester), tester.Id);
		}

		async Task<bool> removeTester(Tester tester)
		{
			var distribution = Database.Main.GetObject<AppClass>(tester.DistributionId);
			var app = Database.Main.GetObject<AppClass>(tester.AppId);
			var response = await Api.Account.RemoveUser1(app.Owner.Name, app.Name, distribution.Name, new MobileCenterApi.Models.DistributionGroupUserRequest { UserEmails = new string[] { tester.User.Email } });
			return true;
		}

		#endregion //Distribution

		#region Crashes
		public Task SyncCrashGroups(AppClass app)
		{
			return RunSingularTask(() => syncCrashGroups(app), app.Id);
		}

		async Task syncCrashGroups(AppClass app)
		{
			var response = await Api.Crash.List4(app.Owner.Name, app.Name);
			var crashes = new List<CrashGroup>();
			var stacks = new List<ReasonStackFrame>();
			foreach (var r in response)
			{
				var crash = r.ToCrashGroup(app);
				crashes.Add(crash);
				if(r.ReasonFrame != null)
					stacks.Add(r.ReasonFrame.ToReasonStackFrame(crash));
			}
			Database.Main.InsertOrReplaceAll(crashes);
			Database.Main.InsertOrReplaceAll(stacks);
		}

		public Task SyncStackTrace(CrashGroup crashGroup) => RunSingularTask(()=>syncStackTrace(crashGroup),crashGroup.Id);

		async Task syncStackTrace(CrashGroup crashGroup)
		{
			var app = Database.Main.GetObject<AppClass>(crashGroup.AppId);
			var resp = await Api.Crash.GetStacktrace(crashGroup.Id, app.Owner.Name, app.Name);
			var stack = resp.ToStackTrace(crashGroup);
			Database.Main.InsertOrReplace(stack);
		}


		#endregion //Crashes
	}
}
