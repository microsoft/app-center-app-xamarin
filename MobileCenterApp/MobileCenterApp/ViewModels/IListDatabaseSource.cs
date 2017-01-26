using System;
using System.Collections;
using System.Collections.Generic;

namespace MobileCenterApp
{
	public class IListDatabaseSource<T> : IList<IList<T>> where T : new()
	{
		public SimpleDatabase.SimpleDatabaseConnection Database { get; set; }
		public IList<T> this[int index]
		{
			get
			{
				return new GroupedList<T>(Database, index)
				{
					Display = Database.SectionHeader<T>(index),
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


		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public void Add(IList<T> item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			Database?.ClearMemory<T>();
		}

		public bool Contains(IList<T> item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(IList<T>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<IList<T>> GetEnumerator()
		{
			int position = -1;
			while (position < Count - 1)
			{
				position++;
				yield return this[position];
			}
		}

		public int IndexOf(IList<T> item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, IList<T> item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(IList<T> item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			int position = -1;
			while (position < Count -1)
			{
				position++;
				yield return this[position];
			}
		}


	}
	class GroupedList<T> : IList<T> where T : new ()
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

		public T this[int index]
		{
			get
			{
				return Database.ObjectForRow<T>(Section, index);
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
				return Database?.RowsInSection<T>(Section) ?? 0;
			}
		}


		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}


		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(T item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator()
		{
			int position = -1;
			while (position < Count - 1)
			{
				position++;
				yield return this[position];
			}
		}

		public int IndexOf(T item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			int position = -1;
			while (position < Count - 1)
			{
				position++;
				yield return this[position];
			}
		}
	}
}
