using System;
using System.Collections.Generic;
using System.Linq;

using SQLite.Net;
using SQLite.Net.Interop;

namespace TabataPCL
{
	public class TabataRepository : IDisposable
	{
		private SQLiteConnection _db;
		private object lockGuard = new object();

		public TabataRepository (IDataPlatform dbInfo)
		{
			if (_db == null) {
				_db = new SQLiteConnection (dbInfo.SQLitePlatform, dbInfo.DBFile, true);

				// Create the tabata table - if the table already exists, won't recreate it
				_db.CreateTable<Tabata> ();
			}
		}

		public void InsertTabata (Tabata tabataToInsert)
		{
			lock (lockGuard) {
				_db.Insert (tabataToInsert);
			}
		}

		public List<Tabata> RetrieveAllTabatas ()
		{			
			return _db.Table<Tabata> ().ToList ();
		}

		public Tabata GetTabataById (int tabataId)
		{
			return _db.Table<Tabata> ().Where (t => t.TabataID == tabataId).FirstOrDefault ();
		}

		public void Dispose ()
		{
			if (_db != null)
				_db.Close ();
		}

	}
}

