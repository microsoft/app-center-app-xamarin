using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SimpleAuth;

namespace MobileCenterApp
{
	public static class Settings
	{
		public static string CurrentAppId {
			get { return GetString (); }
			set { 
				SetString (value);
				NotificationManager.Shared.ProcAppsChanged(value);
			}
		}
		public static AppClass CurrentApp
		{
			get {
				var appId = CurrentAppId;
				return string.IsNullOrWhiteSpace(appId) ? null : Database.Main.GetObject<AppClass>(appId);
			}
			set
			{
				CurrentAppId = value?.Id;
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

		static User currentUser;
		public static User CurrentUser
		{
			get
			{
				if (currentUser != null)
					return currentUser;
				var json = GetString();
				if (string.IsNullOrWhiteSpace(json))
					return null;
				return currentUser = json.ToObject<User>();
			}
			set
			{
				currentUser = value;
				var json = value?.ToJson() ?? "";
				SetString(json);
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


