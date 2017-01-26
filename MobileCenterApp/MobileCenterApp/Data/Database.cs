using System;
using SimpleDatabase;
using System.IO;
using MobileCenterApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileCenterApp
{
	internal class Database : SimpleDatabaseConnection
	{
		public static Database Main { get; set; } = new Database();
		static string dbPath => Path.Combine(Locations.LibDir, "db.db");

		public Database() : base(dbPath)
		{
			CreateTables(
				typeof(AppClass),
				typeof(Owner)
				);

			this.MakeClassInstant<AppClass>();
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
	}
}

