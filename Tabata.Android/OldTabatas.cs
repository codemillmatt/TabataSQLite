
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

using TabataPCL;

namespace Tabata.Android
{
	[Activity (Label = "Previous Tabatas")]			
	public class OldTabatas : ListActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var repo = new TabataRepository (new SQLiteInfo ());

			var allTabatas = repo.RetrieveAllTabatas ();

			ListAdapter = new TabataAdapter (this, allTabatas);
				
		}
	}
}

