using System;
namespace MobileCenterApp
{
	public class EventArgs<T> : EventArgs
	{
		private readonly T data;

		public EventArgs(T data)
		{
			this.data = data;
		}

		public T Data => data;

	}
}
