using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace multilibs
{
	public class ActiveGamesTableSource : TableSource
	{
		public event TestHandler GameClicked;
		public delegate void TestHandler(string gameName);

		public ActiveGamesTableSource (): base(new List<TableItemGroup>())
		{			
			TableItemGroup tGroup;
			
			// My games Section
			tGroup = new TableItemGroup() { Name = "My Games"};
			tGroup.Items.Add("Game 1");
			tGroup.Items.Add("Game 2");
			tableItems.Add(tGroup);
			
			// Web games Section
			tGroup = new TableItemGroup{ Name = "Web Games"};
			tGroup.Items.Add("Web Game 1");
			tGroup.Items.Add("Web Game 2");
			tGroup.Items.Add("Web Game 3");
			tableItems.Add(tGroup);
		}

		public void AddGame(string gameName)
		{
			tableItems.Single(i => i.Name == "My Games").Items.Add(gameName);
		}

		public override string TitleForFooter (UITableView tableView, int section)
		{
			return string.Format("{0} Games", tableItems[section].Items.Count);
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (GameClicked != null) {
				GameClicked(tableItems[indexPath.Section].Items[indexPath.Row]);
			}
			tableView.DeselectRow (indexPath, true); // normal iOS behaviour is to remove the blue highlight
		}
	}
}

