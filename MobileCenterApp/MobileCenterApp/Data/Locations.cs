using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobileCenterApp.Data
{
	internal static class Locations
	{
		static Locations()
		{
			Directory.CreateDirectory(Locations.DocumentsDir);
			Directory.CreateDirectory(Locations.LibDir);
			Directory.CreateDirectory(Locations.ImageCache);
		}



#if __OSX__
		[System.Runtime.InteropServices.DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
		public static extern IntPtr NSHomeDirectory();

		public static string ContainerDirectory {
			get {
				var val = ((Foundation.NSString)ObjCRuntime.Runtime.GetNSObject(NSHomeDirectory())).ToString ();

				if(val.Contains("Container"))
					return val;
				return Path.Combine (val, "Library/Application Support/MobileCenterApp");
			}
		}
		public static string BaseDir = ContainerDirectory;
#else
		public static string BaseDir = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).ToString();
#endif
		

		public static readonly string LibDir = Path.Combine(BaseDir, "Library/");
		public static readonly string DocumentsDir = Path.Combine(BaseDir, "Documents/");
		public static readonly string ImageCache = Path.Combine(LibDir, "Images");
	}
}