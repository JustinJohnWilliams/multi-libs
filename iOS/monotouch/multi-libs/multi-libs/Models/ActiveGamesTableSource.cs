using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using RestfulAdapter;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace multilibs
{
	public class ActiveGamesTableSource : TableSource
	{


		public ActiveGamesTableSource (): base(new List<TableItemGroup>())
		{
		}

		public ActiveGamesTableSource (List<TableItemGroup> items) : base(items)
		{

		}

		public override string TitleForFooter (UITableView tableView, int section)
		{
			return string.Format("{0} Games", tableItems[section].Items.Count);
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			base.RowSelected (tableView, indexPath);
			tableView.DeselectRow (indexPath, true); // normal iOS behaviour is to remove the blue highlight
		}
	}
}

