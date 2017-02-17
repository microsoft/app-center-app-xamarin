using System;
using System.Collections.Generic;
using MobileCenterApp;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public static class SimpleIoC
	{
		static readonly Dictionary<Type, Type> RegisteredTypes = new Dictionary<Type, Type>();

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

		public static Page GetPage(BaseViewModel model)
		{
			var page = GetPage(model.GetType());
			if (page == null)
			{
				throw new NotImplementedException($"There is no Page registered with {model.GetType()}. Please register the page and view model with SimpleIoC");
			}
			page.BindingContext = model;
			return page;
		}

		public static T GetPage<T>(BaseViewModel model) where T : Page
		{
			var page = GetPage(model);
			return (T)page;
		}

		public static Page GetPage<T>()
		{
			return GetObject<T, Page>();
		}

		public static Page GetPage(Type type)
		{
			return GetObject<Page>(type);
		}

		public static T GetObject<T>()
		{
			return GetObject<T, T>();
		}
		public static T1 GetObject<T, T1>()
		{
			return GetObject<T1>(typeof(T));
		}
		public static T GetObject<T>(Type type)
		{
			Type objectType;
			if (!RegisteredTypes.TryGetValue(type, out objectType))
				return default(T);

			return (T)Activator.CreateInstance(objectType);
		}
	}
}

