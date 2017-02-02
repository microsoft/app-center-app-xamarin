using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleAuth;
namespace MobileCenterApi
{
	public class MobileCenterAuthenticator : BasicAuthAuthenticator
	{
		public MobileCenterAuthenticator(HttpClient client, string loginUrl) : base(client, loginUrl)
		{

		}
		public override async Task<bool> VerifyCredentials(string username, string password)
		{
			if (string.IsNullOrWhiteSpace(username))
				throw new System.Exception("Invalid Username");
			if (string.IsNullOrWhiteSpace(password))
				throw new System.Exception("Invalid Password");

			var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
			var request = new HttpRequestMessage(HttpMethod.Post, loginUrl);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", key);
			//request.Headers.Add("Accept", "application/json");
			request.Content = new StringContent("{\"description\": \"SimpleAuth\"}", Encoding.UTF8, "application/json");
			var response = await client.SendAsync(request);
			var respString = await response.Content.ReadAsStringAsync();
			response.EnsureSuccessStatusCode();
			var respObj = await respString.ToObjectAsync<ApiTokensCreateResponse>();
			FoundAuthCode(respObj.ApiToken);
			return true;
		}

	}

	public partial class MobileCenterAPIServiceApiKeyApi : SimpleAuth.BasicAuthApi
	{

		public MobileCenterAPIServiceApiKeyApi(string apiKey, string encryptionKey, System.Net.Http.HttpMessageHandler handler = null) :
				base(apiKey, encryptionKey, "v0.1/api_tokens", handler)
		{
			BaseAddress = new System.Uri("https://api.mobile.azure.com"); ;
		}

		public override Task PrepareClient(HttpClient client)
		{
			client.DefaultRequestHeaders.Add("X-API-Token", CurrentBasicAccount.Key);
			return Task.FromResult(true);
		}
		protected override BasicAuthAuthenticator CreateAuthenticator()
		{
			return new MobileCenterAuthenticator(Client, LoginUrl);
		}

		public override void OnException(object sender, System.Exception ex)
		{
			base.OnException(sender, ex);
		}
		protected override T Deserialize<T>(string data)
		{
			try
			{
				if (Verbose)
					Console.WriteLine(data);
				return base.Deserialize<T>(data);
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(data);
				throw ex;
			}
		}
		protected override T Deserialize<T>(string data, object inObject)
		{
			try
			{
				if (Verbose)
					Console.WriteLine(data);
				return base.Deserialize<T>(data, inObject);
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(data);
				throw ex;
			}
		}
		[Path("/v0.1/apps/{owner_name}/{app_name}/commits/batch/{sha_collection}")]
		public virtual Task<CommitsResult[]> GetCommits(string sha_collection, string owner_name, string app_name, string form = "full")
		{
			var queryParameters = new Dictionary<string, string> { { "sha_collection", sha_collection }, { "owner_name", owner_name }, { "app_name", app_name }, { "form", form } };
			return Get<CommitsResult[]>(queryParameters: queryParameters, authenticated: true);
		}
	}

	public partial class CommitsResult
	{
		public string Sha { get; set; }
		public Commit Commit { get; set; }
		public Owner Author { get; set; }
	}

	public partial class BranchStatus
	{
		[JsonProperty("branch")]
		public Branch Branch { get; set; }
	}

	public partial class Commit
	{
		[JsonProperty("message")]
		public string Message { get; set; }

		public Owner Author { get; set; }
	}

	public partial class SourceRepository
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("full_name")]
		public string FullName { get; set; }

		[JsonProperty("owner")]
		public SourceRepositoryOwner Owner { get; set; }

		[JsonProperty("private")]
		public bool Private { get; set; }

		[JsonProperty("html_url")]
		public string HtmlUrl { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("fork")]
		public bool Fork { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("forks_url")]
		public string ForksUrl { get; set; }

		[JsonProperty("keys_url")]
		public string KeysUrl { get; set; }

		[JsonProperty("collaborators_url")]
		public string CollaboratorsUrl { get; set; }

		[JsonProperty("teams_url")]
		public string TeamsUrl { get; set; }

		[JsonProperty("hooks_url")]
		public string HooksUrl { get; set; }

		[JsonProperty("issue_events_url")]
		public string IssueEventsUrl { get; set; }

		[JsonProperty("events_url")]
		public string EventsUrl { get; set; }

		[JsonProperty("assignees_url")]
		public string AssigneesUrl { get; set; }

		[JsonProperty("branches_url")]
		public string BranchesUrl { get; set; }

		[JsonProperty("tags_url")]
		public string TagsUrl { get; set; }

		[JsonProperty("blobs_url")]
		public string BlobsUrl { get; set; }

		[JsonProperty("git_tags_url")]
		public string GitTagsUrl { get; set; }

		[JsonProperty("git_refs_url")]
		public string GitRefsUrl { get; set; }

		[JsonProperty("trees_url")]
		public string TreesUrl { get; set; }

		[JsonProperty("statuses_url")]
		public string StatusesUrl { get; set; }

		[JsonProperty("languages_url")]
		public string LanguagesUrl { get; set; }

		[JsonProperty("stargazers_url")]
		public string StargazersUrl { get; set; }

		[JsonProperty("contributors_url")]
		public string ContributorsUrl { get; set; }

		[JsonProperty("subscribers_url")]
		public string SubscribersUrl { get; set; }

		[JsonProperty("subscription_url")]
		public string SubscriptionUrl { get; set; }

		[JsonProperty("commits_url")]
		public string CommitsUrl { get; set; }

		[JsonProperty("git_commits_url")]
		public string GitCommitsUrl { get; set; }

		[JsonProperty("comments_url")]
		public string CommentsUrl { get; set; }

		[JsonProperty("issue_comment_url")]
		public string IssueCommentUrl { get; set; }

		[JsonProperty("contents_url")]
		public string ContentsUrl { get; set; }

		[JsonProperty("compare_url")]
		public string CompareUrl { get; set; }

		[JsonProperty("merges_url")]
		public string MergesUrl { get; set; }

		[JsonProperty("archive_url")]
		public string ArchiveUrl { get; set; }

		[JsonProperty("downloads_url")]
		public string DownloadsUrl { get; set; }

		[JsonProperty("issues_url")]
		public string IssuesUrl { get; set; }

		[JsonProperty("pulls_url")]
		public string PullsUrl { get; set; }

		[JsonProperty("milestones_url")]
		public string MilestonesUrl { get; set; }

		[JsonProperty("notifications_url")]
		public string NotificationsUrl { get; set; }

		[JsonProperty("labels_url")]
		public string LabelsUrl { get; set; }

		[JsonProperty("releases_url")]
		public string ReleasesUrl { get; set; }

		[JsonProperty("deployments_url")]
		public string DeploymentsUrl { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt { get; set; }

		[JsonProperty("pushed_at")]
		public DateTime? PushedAt { get; set; }

		[JsonProperty("git_url")]
		public string GitUrl { get; set; }

		[JsonProperty("ssh_url")]
		public string SshUrl { get; set; }

		[JsonProperty("clone_url")]
		public string CloneUrl { get; set; }

		[JsonProperty("svn_url")]
		public string SvnUrl { get; set; }

		[JsonProperty("homepage")]
		public string Homepage { get; set; }

		[JsonProperty("size")]
		public int Size { get; set; }

		[JsonProperty("stargazers_count")]
		public int StargazersCount { get; set; }

		[JsonProperty("watchers_count")]
		public int WatchersCount { get; set; }

		[JsonProperty("language")]
		public string Language { get; set; }

		[JsonProperty("has_issues")]
		public bool HasIssues { get; set; }

		[JsonProperty("has_downloads")]
		public bool HasDownloads { get; set; }

		[JsonProperty("has_wiki")]
		public bool HasWiki { get; set; }

		[JsonProperty("has_pages")]
		public bool HasPages { get; set; }

		[JsonProperty("forks_count")]
		public int ForksCount { get; set; }

		[JsonProperty("mirror_url")]
		public object MirrorUrl { get; set; }

		[JsonProperty("open_issues_count")]
		public int OpenIssuesCount { get; set; }

		[JsonProperty("forks")]
		public int Forks { get; set; }

		[JsonProperty("open_issues")]
		public int OpenIssues { get; set; }

		[JsonProperty("watchers")]
		public int Watchers { get; set; }

		[JsonProperty("default_branch")]
		public string DefaultBranch { get; set; }

		[JsonProperty("permissions")]
		public Permissions Permissions { get; set; }

	}
	public partial class Permissions
	{

		[JsonProperty("admin")]
		public bool Admin { get; set; }

		[JsonProperty("push")]
		public bool Push { get; set; }

		[JsonProperty("pull")]
		public bool Pull { get; set; }
	}

	public partial class SourceRepositoryOwner
	{

		[JsonProperty("login")]
		public string Login { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("avatar_url")]
		public string AvatarUrl { get; set; }

		[JsonProperty("gravatar_id")]
		public string GravatarId { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("html_url")]
		public string HtmlUrl { get; set; }

		[JsonProperty("followers_url")]
		public string FollowersUrl { get; set; }

		[JsonProperty("following_url")]
		public string FollowingUrl { get; set; }

		[JsonProperty("gists_url")]
		public string GistsUrl { get; set; }

		[JsonProperty("starred_url")]
		public string StarredUrl { get; set; }

		[JsonProperty("subscriptions_url")]
		public string SubscriptionsUrl { get; set; }

		[JsonProperty("organizations_url")]
		public string OrganizationsUrl { get; set; }

		[JsonProperty("repos_url")]
		public string ReposUrl { get; set; }

		[JsonProperty("events_url")]
		public string EventsUrl { get; set; }

		[JsonProperty("received_events_url")]
		public string ReceivedEventsUrl { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("site_admin")]
		public bool SiteAdmin { get; set; }
	}
}
