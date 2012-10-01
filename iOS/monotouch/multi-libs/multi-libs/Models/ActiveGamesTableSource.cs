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
		public event TestHandler GameClicked;
		public delegate void TestHandler(string gameName);

		private String baseUri = "http://localhost:3000/";
//		private String baseUri = "http://dry-peak-5299.herokuapp.com/";

		public ActiveGamesTableSource (): base(new List<TableItemGroup>())
		{
		}

		public ActiveGamesTableSource (List<TableItemGroup> items) : base(items)
		{

		}

		public void AddGame(Guid gameId, string gameName)
		{

			var restFacilitator = new RestFacilitator();			
			var restService = new RestService(restFacilitator, baseUri);			
			var asyncDelegation = new AsyncDelegation(restService);			
			asyncDelegation
				.Post("add", new { id = gameId.ToString(), name=gameName })
					.WhenFinished(() => { });
			asyncDelegation.Go();
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

