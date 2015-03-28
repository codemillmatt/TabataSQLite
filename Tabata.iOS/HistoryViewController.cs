using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;
using System.IO;

namespace Tabata.iOS
{
	partial class historyViewController : UITableViewController
	{
		TabataPCL.AllTabatas _allTabatas;

		public historyViewController (IntPtr handle) : base (handle)
		{
		}

		private string GetFileName()
		{
			var docPath = MonoTouch.Foundation.NSFileManager.DefaultManager.GetUrls(
				MonoTouch.Foundation.NSSearchPathDirectory.DocumentDirectory,
				MonoTouch.Foundation.NSSearchPathDomain.User)[0];

			return Path.Combine(docPath.Path, "data.csv");
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.TableView.ContentInset = new UIEdgeInsets (20, 0, 0, 0);

			var fileName = this.GetFileName ();

			_allTabatas = new TabataPCL.AllTabatas ();

			using (var sr = new StreamReader (fileName)) {
				_allTabatas.PopulateTabatas (sr);
			}				

			this.TableView.Source = new TabataSource (_allTabatas);
		}
	}

	public class TabataSource : UITableViewSource
	{
		string _cellIdentifer = "cell";
		TabataPCL.AllTabatas _allTabatas;

		public TabataSource (TabataPCL.AllTabatas tabatas)
		{
			_allTabatas = tabatas;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _allTabatas.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (_cellIdentifer);

			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, _cellIdentifer);
			}

			var currentTabata = _allTabatas [indexPath.Row] as TabataPCL.Tabata;

			cell.TextLabel.Text = string.Format ("{0} - {1} sets",
				currentTabata.TabataDate.ToShortDateString (), currentTabata.NumberOfSets);

			cell.DetailTextLabel.Text = string.Format ("{0} sec work, {1} sec rest", currentTabata.WorkInterval,
				currentTabata.RestInterval);

			return cell;
		}
	}
}
