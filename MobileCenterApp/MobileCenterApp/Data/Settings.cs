using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MobileCenterApp
{
	public static class Settings
	{
		public static string CurrentApp {
			get { return GetString (); }
			set { 
				SetString (value);
				NotificationManager.Shared.ProcAppsChanged(value);
			}
		}

		public static bool IsOfflineMode
		{
			get { return GetBool();}
			set { 
				SetBool(value); 
				NotificationManager.Shared.ProcOffineModeChanged(value);
			}
		}

		#region Helpers
		public static ISettings AppSettings { get; } = CrossSettings.Current;

		public static string GetString (string defaultValue = "", [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetString (string value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<string> (memberName, value);
		}
		public static int GetInt (int defaultValue = 0, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetInt (int value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<int> (memberName, value);
		}

		public static long GetLong (long defaultValue = 0, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetLong (long value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<long> (memberName, value);
		}

		public static bool GetBool (bool defaultValue = false, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetBool (bool value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<bool> (memberName, value);
		}


		static T Get<T> (T defaultValue, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		static void Set<T> (T value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<T> (memberName, value);
		}
		#endregion
	}
}


