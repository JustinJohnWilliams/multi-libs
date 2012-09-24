
using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace multilibs
{
	public partial class HomeViewController : UIViewController
	{
		public HomeViewController () : base ("HomeViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Games", "Games");
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.NavigationController.SetNavigationBarHidden (true, animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			this.NavigationController.SetNavigationBarHidden (false, animated);
		}

		partial void CreateClicked (NSObject sender)
		{
			var gameView = new GameViewController();
			this.NavigationController.PushViewController(gameView, true);
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.

			var tableItems = new List<TableItemGroup>();

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

			GamesTable.Source = new TableSource(tableItems);
			Add (GamesTable);
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

