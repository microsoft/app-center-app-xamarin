using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileCenterApp
{
	public class BaseModel : INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public BaseModel()
		{
			
		}


		/// <summary>
		///     Dispose method.
		/// </summary>
		public void Dispose()
		{
			ClearEvents();
		}

		internal bool ProcPropertyChanged<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			return PropertyChanged.SetProperty(this, ref currentValue, newValue, propertyName);
		}

		internal void ProcPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public void ClearEvents()
		{
			if (PropertyChanged == null)
				return;
			var invocation = PropertyChanged.GetInvocationList();
			foreach (var p in invocation)
				PropertyChanged -= (PropertyChangedEventHandler)p;
		}

		public static string GetIndexChar(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return "#";
			var theChar = name[0];
			return char.IsLetter(theChar) ? name.Substring(0, 1).ToUpper() : "#";
		}
	}
}
