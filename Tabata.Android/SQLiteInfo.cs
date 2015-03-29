using System;
using System.IO;

using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;

using TabataPCL;

namespace Tabata.Android
{
	public class SQLiteInfo : IDataPlatform
	{
		public SQLiteInfo ()
		{
		}

		public string DBFile {
			get {
				return Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments), "data.db3");
			}
		}

		public ISQLitePlatform SQLitePlatform {
			get {
				return new SQLitePlatformAndroid (); 
			}
		}
	}
}

