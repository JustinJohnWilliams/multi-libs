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
		public ActiveGamesTableSource (): base(new List<TableItemGroup>())
		{			
			TableItemGroup tGroup;
			
			// My games Section
			tGroup = new TableItemGroup() { Name = "My Games"};
			tGroup.Items.Add("Game 1");
			tGroup.Items.Add("Game 2");
			tGroup.Footer = string.Format("{0} Games", tGroup.Items.Count);
			tableItems.Add(tGroup);
			
			// Web games Section
			tGroup = new TableItemGroup{ Name = "Web Games"};
			tGroup.Items.Add("Web Game 1");
			tGroup.Items.Add("Web Game 2");
			tGroup.Items.Add("Web Game 3");
			tGroup.Footer = string.Format("{0} Games", tGroup.Items.Count);
			tableItems.Add(tGroup);
		}

		public void AddGame(string gameName)
		{
			tableItems.Single(i => i.Name == "My Games").Items.Add(gameName);
		}
	}

}

