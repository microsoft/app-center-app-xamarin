using System;
using SimpleDatabase;
using SQLite;

namespace MobileCenterApp
{
	public class AppClass : BaseModel
	{
		[PrimaryKey]
		public string Id { get; set; }

		[GroupBy]
		public string IndexCharacter { get; set; }

		public string AppSecret { get; set; }

		public string Description { get; set; }

		[OrderBy]
		public string DisplayName { get; set; }

		[Indexed]
		public string Name { get; set; }

		string os;
		public string Os { 
			get { return os;} 
			set { ProcPropertyChanged(ref os, value); }
		}

		public string Platform { get; set; }

		public string IconUrl { get; set; }

		public string OwnerId { get; set; }

		[Ignore]
		public Owner Owner 
		{
			get {
				 return Database.Main.GetObject<Owner>(OwnerId);
			}
			set {
				if(value?.Id != OwnerId)
					ProcPropertyChanged (nameof(Owner));
				OwnerId = value?.Id;
			}
		}

		public string AzureSubscriptionId { get; set; }

		[Indexed]
		public DateTime DateImported { get; set; }
	}
}
