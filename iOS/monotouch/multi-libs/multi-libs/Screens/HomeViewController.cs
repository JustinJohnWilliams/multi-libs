
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
//		private String baseUri = "http://dry-peak-5299.herokuapp.com/";
		private RestFacilitator restFacilitator;
		private RestService restService;
		private bool shouldPool;

		public HomeViewController () : base ("HomeViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Games", "Games");
			restFacilitator = new RestFacilitator();
			restService = new RestService(restFacilitator, baseUri);
			shouldPool = false;
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
			shouldPool = true;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			_games = new List<TableItemGroup>();
			_activeGames = new ActiveGamesTableSource(_games);
			GamesTable.Source = _activeGames;
			Add (GamesTable);

			_activeGames.RowClicked += (gameId) => {
				JoinGame(gameId);
			};
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			shouldPool = true;
			PollGames();
		}

		partial void CreateClicked (NSObject sender)
		{
			var gameId = Guid.NewGuid().ToString();
			var gameName = "Mono Game "+ gameId.Substring(0, 5);
			AddGame(gameId,gameName);
		}

		partial void RefreshClicked(NSObject sender)
		{
			FetchGames();
		}

		void JoinGame (string gameId)
		{
			var asyncDelegation = new AsyncDelegation(restService);
			asyncDelegation.Post("joingame", new {gameId = gameId, playerId = Application.PlayerId, playerName = "Mono Touch"})
				.WhenFinished(()=> {
					var gameView = new GameViewController(gameId);
					this.NavigationController.PushViewController(gameView, true);
				});
			asyncDelegation.Go();
		}

		private void FetchGames()
		{
			var asyncDelegation = new AsyncDelegation(restService);
			asyncDelegation.Get<Hashes>("list", new { q = "list" })
				.WhenFinished(
					result =>
					{					
					// Web games Section
					var tGroup = new TableItemGroup{ Name = "Active Games"};
					foreach(var hash in result)
					{
						tGroup.Items.Add(hash["name"].ToString());
						tGroup.ItemIds.Add(hash["id"].ToString());
					}
					_games.Clear();
					_games.Add(tGroup);
					InvokeOnMainThread(GamesTable.ReloadData);
				});			
			asyncDelegation.Go();
		}

		private void PollGames ()
		{
			if(!shouldPool)
				return;

			FetchGames ();
			NSTimer.CreateScheduledTimer (5.0, delegate {
				PollGames ();
			});
		}

		private void AddGame(string gameId, string gameName)
		{		
			var asyncDelegation = new AsyncDelegation(restService);			
			asyncDelegation
				.Post("add", new { id = gameId, name=gameName })
					.WhenFinished(()=>{JoinGame(gameId);})
					.Go();			
		}
	}
}

