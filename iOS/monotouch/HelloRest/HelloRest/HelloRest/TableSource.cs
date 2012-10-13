using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace HelloRest
{
	public class TableSource : UITableViewSource
	{
		protected List<String> tableItems;
		protected string cellIdentifier = "CELL_IDENTIFIER";

		public TableSource() : this(new List<String>()){}
		public TableSource(List<String> items)
		{
			this.tableItems = items;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (this.cellIdentifier);
			
			// if there are no cell to reuse, create a new one
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, this.cellIdentifier);
			}
			
			// set the item text
			cell.TextLabel.Text = tableItems[indexPath.Row];

			return cell;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, true); // normal iOS behaviour is to remove the blue highlight

			var alert = new UIAlertView();
			alert.Message = string.Format("{0} tapped!", tableItems[indexPath.Row]);
			alert.AddButton("OK");
			alert.Show();
		}
	}
}

