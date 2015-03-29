using System;
using SQLite.Net.Interop;

namespace TabataPCL
{
	public interface IDataPlatform
	{
		string DBFile { get; }
		ISQLitePlatform SQLitePlatform { get; }
	}
}

