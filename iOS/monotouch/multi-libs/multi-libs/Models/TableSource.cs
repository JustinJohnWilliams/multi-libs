using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace multilibs
{
	public class TableSource : UITableViewSource
	{
		protected List<TableItemGroup> tableItems;
		protected string cellIdentifier = "TableCell";

		public event RowClickedHandler RowClicked;
		public delegate void RowClickedHandler(string rowId);

		public TableSource(List<TableItemGroup> items)
		{
			this.tableItems = items;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return this.tableItems.Count;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return this.tableItems[section].Items.Count;
		}

		public override string TitleForHeader (UITableView tableView, int section)
		{
			return this.tableItems[section].Name;
		}

		public override string TitleForFooter (UITableView tableView, int section)
		{
//			return this.tableItems[section].Footer;
			return string.Empty;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (this.cellIdentifier);

			// if there are no cell to reuse, create a new one
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, this.cellIdentifier);
			}

			// set the item text
			cell.TextLabel.Text = this.tableItems[indexPath.Section].Items[indexPath.Row];

			// set arrow indicator
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (RowClicked != null) {
				RowClicked(tableItems[indexPath.Section].ItemIds[indexPath.Row]);
			}
		}
	}
}

