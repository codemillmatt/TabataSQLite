using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Generic;

using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;

using TabataPCL;

namespace Tabata.iOS
{
	partial class historyViewController : UITableViewController
	{
		public historyViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			var repo = new TabataRepository (new SQLiteInfoIOS ());

			this.TableView.Source = new TabataSource (repo.RetrieveAllTabatas ());

			this.TableView.ReloadData ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.TableView.ContentInset = new UIEdgeInsets (20, 0, 0, 0);
		}
	}

	public class TabataSource : UITableViewSource
	{
		string _cellIdentifer = "cell";
		List<TabataPCL.Tabata> _allTabatas;

		public TabataSource (List<TabataPCL.Tabata> tabatas)
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
