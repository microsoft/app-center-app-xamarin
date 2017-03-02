using System;
using SimpleDatabase;
using System.IO;
using MobileCenterApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

namespace MobileCenterApp
{
	internal class Database : SimpleDatabaseConnection
	{
		static Database main;
		public static Database Main {
			get {
				try
				{
					if(main == null)
						main = new Database();
				}
				catch (Exception ex)
				{
					LogManager.Shared.Report(ex);
					//If the database is bad, delete and start over!
					File.Delete(dbPath);
					Task.Delay(100).Wait();
					main = new Database();
				}
				return main;
			}
			set { main = value; }
		}
		static string dbPath => Path.Combine(Locations.LibDir, "db.db");

		public Database() : base(dbPath)
		{
			CreateTables(
				typeof(AppClass),
				typeof(Owner),
				typeof(Branch),
				typeof(CommitClass),
				typeof(Build),
				typeof(RepoConfig),
				typeof(Release),
				typeof(DistributionGroup),
				typeof(Tester),
				typeof(User),
				typeof(DistributionReleaseGroup),
				typeof(CrashGroup),
				typeof(ReasonStackFrame),
				typeof(StackTrace)
			);
		}

		public T GetObject<T, T1>(object id) where T1 : T, new() where T : new()
		{
			var obj = GetObject<T>(id);
			return EqualityComparer<T>.Default.Equals(obj, default(T)) ? GetObject<T1>(id) : obj;
		}

		public Task ResetTable<T>()
		{
			var tcs = new TaskCompletionSource<bool>();
			this.RunInTransaction((con) =>
			{
				con.DropTable<T>();
				con.CreateTable<T>();
				this.ClearMemory<T>();
				tcs.TrySetResult(true);
				   
			});
			return tcs.Task;
		}

		public int InsertOrIgnore(object obj)
		{
			return this.Insert(obj, "OR IGNORE");
		}
		public int InsertOrIgnoreAll(IEnumerable objects)
		{
			return this.InsertAll(objects, "OR IGNORE");
		}
	}
}

