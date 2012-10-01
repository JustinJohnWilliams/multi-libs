
using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RestfulAdapter;

namespace multilibs
{
	public partial class HomeViewController : UIViewController
	{
		private ActiveGamesTableSource _activeGames;
		private List<TableItemGroup> _games;
		private String baseUri = "http://localhost:3000/";

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
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			_games = new List<TableItemGroup>();
			_activeGames = new ActiveGamesTableSource(_games);

			_activeGames.GameClicked += (gameName) => {
				var gameView = new GameViewController(gameName);
				this.NavigationController.PushViewController(gameView, true);
			};
			GamesTable.Source = _activeGames;
			Add (GamesTable);

			this.InvokeOnMainThread(GamesTable.ReloadData);			
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

		partial void CreateClicked (NSObject sender)
		{
			var gameId = Guid.NewGuid();
			var gameName = "MonoTouch Game "+ gameId.ToString().Substring(0, 5);
			_activeGames.AddGame(gameId,gameName);

			var gameView = new GameViewController(gameName);
			this.NavigationController.PushViewController(gameView, true);
			GamesTable.ReloadData();
		}

		partial void RefreshClicked(NSObject sender)
		{
			FetchData();
		}

		public void FetchData()
		{
			_games = new List<TableItemGroup>();
			
			// Web games Section
			var tGroup = new TableItemGroup{ Name = "Active Games"};
			_games.Add(tGroup);

			var restFacilitator = new RestFacilitator();			
			var restService = new RestService(restFacilitator, baseUri);			
			var asyncDelegation = new AsyncDelegation(restService);
			asyncDelegation.Get<Hashes>("list", new { q = "list" })
				.WhenFinished(
					result =>
					{
					foreach(var hash in result)
					{
						tGroup.Items.Add(hash["name"].ToString());
					}
					
					this.InvokeOnMainThread(GamesTable.ReloadData);	
				});			
			asyncDelegation.Go();

		}
	}
}

