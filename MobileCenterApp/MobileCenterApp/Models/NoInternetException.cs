using System;
namespace MobileCenterApp
{
	public class NoInternetException : Exception
	{
		public NoInternetException() : base("No Internet")
		{
		}

	}
}
