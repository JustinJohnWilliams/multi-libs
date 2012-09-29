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
			tGroup = new TableItemGroup() { Name = "My Games"};
			tGroup.Items.Add("Game 1");
			tGroup.Items.Add("Game 2");
			tableItems.Add(tGroup);

			var baseUri = "http://dry-peak-5299.herokuapp.com/";
			
			var restFacilitator = new RestFacilitator();
			
			var restService = new RestService(restFacilitator, baseUri);
			
			var asyncDelegation = new AsyncDelegation(restService);
			
			//call http://search.twitter.com/search.json?q=#haiku
			asyncDelegation.Get<Hash>("search.json", new { q = "#haiku" })
				.WhenFinished(
					result =>
					{
					List<string> tweets = new List<string>();
					textBlockTweets.Text = "";
					foreach (var tweetObject in result["results"].ToHashes())
					{
						textBlockTweets.Text += HttpUtility.HtmlDecode(tweetObject["text"].ToString()) + Environment.NewLine + Environment.NewLine;
					}
				});
			
			asyncDelegation.Go();

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

