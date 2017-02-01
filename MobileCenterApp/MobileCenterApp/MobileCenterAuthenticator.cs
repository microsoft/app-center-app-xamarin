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
			try{
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
}
