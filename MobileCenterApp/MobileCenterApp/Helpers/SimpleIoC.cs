using System;
using System.Collections.Generic;
using MobileCenterApp;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public static class SimpleIoC
	{
		static readonly Dictionary<Type, Type> RegisteredTypes = new Dictionary<Type, Type>();

		static readonly Dictionary<Type, object> Singletons = new Dictionary<Type, object>();

		public static void Register<TType, TType1>() where TType1 : TType
		{
			RegisteredTypes[typeof(TType)] = typeof(TType1);
		}

		public static void Register(Type type, Type type2)
		{
			RegisteredTypes[type] = type2;
		}

		public static void RegisterPage<TType, TType1>() where TType1 : Page where TType : BaseViewModel
		{
			RegisteredTypes[typeof(TType)] = typeof(TType1);
		}

		public static Page GetPage(BaseViewModel model, bool singleton = true)
		{
			var page = GetPage(model.GetType(), singleton);
			page.BindingContext = model;
			return page;
		}

		public static T GetPage<T>(BaseViewModel model, bool singleton = true) where T : Page
		{
			var page = GetPage(model, singleton);
			return (T)page;
		}

		public static Page GetPage<T>(bool singleton = true)
		{
			return GetObject<T, Page>(singleton);
		}

		public static Page GetPage(Type type, bool singleton = true)
		{
			return GetObject<Page>(type, singleton);
		}

		public static T GetObject<T>(bool singleton = true)
		{
			return GetObject<T, T>(singleton);
		}
		public static T1 GetObject<T, T1>(bool singleton = true)
		{
			return GetObject<T1>(typeof(T), singleton);
		}
		public static T GetObject<T>(Type type, bool singleton = true)
		{
			Type objectType;
			if (!RegisteredTypes.TryGetValue(type, out objectType))
				return default(T);
			if (!singleton)
				return (T)Activator.CreateInstance(objectType);
			Object item;
			if (!Singletons.TryGetValue(objectType, out item))
			{
				Singletons[objectType] = item = (T)Activator.CreateInstance(objectType);
			}
			return (T)item;
		}
	}
}

