using System;
using System.IO;

using TabataPCL;

using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;

namespace Tabata.iOS
{
	public class SQLiteInfoIOS : IDataPlatform
	{
		public SQLiteInfoIOS ()
		{
		}

		public string DBFile {
			get {
				var docPath = MonoTouch.Foundation.NSFileManager.DefaultManager.GetUrls(
					MonoTouch.Foundation.NSSearchPathDirectory.LibraryDirectory,
					MonoTouch.Foundation.NSSearchPathDomain.User)[0];

				return Path.Combine(docPath.Path, "data.db3");
			}
		}

		public ISQLitePlatform SQLitePlatform {
			get {
				return new SQLitePlatformIOS ();
			}
		}
	}
}

