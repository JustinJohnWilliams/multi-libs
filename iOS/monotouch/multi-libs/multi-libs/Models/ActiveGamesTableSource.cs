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

		public ActiveGamesTableSource (): base(new List<TableItemGroup>())
		{			
			TableItemGroup tGroup;
			
			// My games Section
//			tGroup = new TableItemGroup() { Name = "My Games"};
//			tGroup.Items.Add("Game 1");
//			tGroup.Items.Add("Game 2");
//			tableItems.Add(tGroup);




			var baseUri = "http://dry-peak-5299.herokuapp.com/";
			
			var restFacilitator = new RestFacilitator();
			
			var restService = new RestService(restFacilitator, baseUri);
			
			var asyncDelegation = new AsyncDelegation(restService);

			asyncDelegation.Get<Hashes>("list", new { q = "list" })
				.WhenFinished(
					result =>
					{
					// Web games Section
					tGroup = new TableItemGroup{ Name = "Web Games"};
					foreach(var hash in result)
					{
						var name = hash["name"].ToString();
						tGroup.Items.Add(name);
					}
					tableItems.Add(tGroup);
				});
			
			asyncDelegation.Go();


//			tGroup.Items.Add("Web Game 1");
//			tGroup.Items.Add("Web Game 2");
//			tGroup.Items.Add("Web Game 3");

		}

		public void AddGame(string gameName)
		{
			tableItems.Single(i => i.Name == "My Games").Items.Add(gameName);

//			var baseUri = "http://dry-peak-5299.herokuapp.com/";
//			
//			var restFacilitator = new RestFacilitator();
//			
//			var restService = new RestService(restFacilitator, baseUri);
//			
//			var asyncDelegation = new AsyncDelegation(restService);
//			
//			asyncDelegation
//				.Post("add")
//					.
//
////			$.post("add", { id: gameId, name: "browser game" }, function() {
////				$.post("joingame", { gameId: gameId, playerId: playerId, playerName: "web guy" }, function() { 
////					window.location.replace("/game?gameId=" + gameId + "&playerId=" + playerId);
////				});
////			});
//			asyncDelegation.Go();
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

