
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

namespace Tabata.Android
{
	[Activity (Label = "Previous Tabatas")]			
	public class OldTabatas : ListActivity
	{
		private string GetFileName() {
			return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),"data.csv");
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var allTabatas = new TabataPCL.AllTabatas ();

			var fileName = GetFileName ();

			// Create your application here
			using (var sr = new StreamReader (fileName)) {			
				allTabatas.PopulateTabatas (sr);
			}
				
			ListAdapter = new TabataAdapter (this, allTabatas);
				
		}
	}
}

