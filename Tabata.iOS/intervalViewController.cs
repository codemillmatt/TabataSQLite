using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;
using System.Timers;
using System.IO;

namespace Tabata.iOS
{
	partial class intervalViewController : UIViewController
	{
		public TabataPCL.Tabata CurrentTabata {
			get;
			set;
		}
			
		public intervalViewController (IntPtr handle) : base (handle)
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Initialize the UI
			timeLeftLabel.Text = this.CurrentTabata.WorkInterval.ToString ();
			currentStateLabel.Text = "Exercise!";

			setsLabel.Text = string.Format ("1 of {0} sets", this.CurrentTabata.NumberOfSets);

			// Start the Tabata
			CurrentTabata.StartTabata (DisplayWorkTimeRemaining, DisplayRestTimeRemaining, SwitchViews, FinishedTabata);
		}
				
		private void DisplayWorkTimeRemaining(string timeLeft)
		{
			InvokeOnMainThread (() => {
				timeLeftLabel.Text = timeLeft;
			});
		}

		private void DisplayRestTimeRemaining(string timeLeft)
		{
			InvokeOnMainThread (() => {
				timeLeftLabel.Text = timeLeft;
			});
		}

		private void SwitchViews(bool showWork, int numberOfCompletedSets)
		{
			InvokeOnMainThread (() => {
				if (showWork) {
					currentStateLabel.Text = "Exercise!";
					timeLeftLabel.Text = this.CurrentTabata.WorkInterval.ToString ();
					setsLabel.Text = string.Format ("{0} of {1} sets", numberOfCompletedSets, this.CurrentTabata.NumberOfSets);
				} else {
					currentStateLabel.Text = "Rest";
					timeLeftLabel.Text = this.CurrentTabata.RestInterval.ToString ();
				}
			});
		}
			
		private void FinishedTabata()
		{
			InvokeOnMainThread (() => {
				// Save the tabatas - create the StreamWriter
				var fileName = this.GetFileName();

				using (var sw = new StreamWriter(fileName, true)) {
					this.CurrentTabata.SaveTabata(sw);
				}
					
				this.NavigationController.PopViewControllerAnimated (true);
			});				
		}	

		private string GetFileName()
		{
			var docPath = MonoTouch.Foundation.NSFileManager.DefaultManager.GetUrls(
				MonoTouch.Foundation.NSSearchPathDirectory.DocumentDirectory,
				MonoTouch.Foundation.NSSearchPathDomain.User)[0];

			return Path.Combine(docPath.Path, "data.csv");
		}

	}
}
