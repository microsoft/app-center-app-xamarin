using System;
using System.Collections;
using System.Collections.Generic;

namespace MobileCenterApp
{
	public class SimpleDatabaseSource<T> : IList where T : new()
	{
		public object this[int index]
		{
			get
			{
				Console.WriteLine($"Loading {index}");
				return new GroupedList<T>(Database, index)
				{
					Display = Database?.SectionHeader<T>(index) ?? "",
				};
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public int Count
		{
			get
			{
				return Database?.NumberOfSections<T>() ?? 0;
			}
		}

		public SimpleDatabase.SimpleDatabaseConnection Database { get; set; }

		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public int Add(object value)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(object value)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public int IndexOf(object value)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		public void Remove(object value)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}


	}
	public class GroupedList<T> : IList where T : new()
	{
		public GroupedList(SimpleDatabase.SimpleDatabaseConnection database, int section)
		{
			Database = database;
			Section = section;
		}
		string display = "";
		public string Display { 
			get
			{
				return display;
			}
			set { display = value; }
		}

		public SimpleDatabase.SimpleDatabaseConnection Database { get; set; }
		public int Section { get; set; }

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		public int Count
		{
			get
			{
				return Database?.RowsInSection<T>(Section) ?? 0;
			}
		}

		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsSynchronized
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public object this[int index]
		{
			get
			{
				Console.WriteLine($"Loading {Section}:{index}");
				return Database.ObjectForRow<T>(Section, index);
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public int Add(object value)
		{
			throw new NotImplementedException();
		}

		public bool Contains(object value)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public int IndexOf(object value)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		public void Remove(object value)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
