
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

		public HomeViewController () : base ("HomeViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Games", "Games");
			restFacilitator = new RestFacilitator();
			restService = new RestService(restFacilitator, baseUri);
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
			GamesTable.Source = _activeGames;
			Add (GamesTable);

			FetchGames();

			_activeGames.GameClicked += (gameId) => {
				var gameView = new GameViewController(gameId);
				this.NavigationController.PushViewController(gameView, true);
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

		partial void CreateClicked (NSObject sender)
		{
			var gameId = Guid.NewGuid();
			var gameName = "Mono Game "+ gameId.ToString().Substring(0, 5);
			AddGame(gameId,gameName);

			var gameView = new GameViewController(gameId);
			this.NavigationController.PushViewController(gameView, true);
		}

		partial void RefreshClicked(NSObject sender)
		{
			FetchGames();
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
						tGroup.ItemIds.Add(new Guid(hash["id"].ToString()));
					}
					_games.Clear();
					_games.Add(tGroup);
					InvokeOnMainThread(GamesTable.ReloadData);
				});			
			asyncDelegation.Go();
		}

		private void AddGame(Guid gameId, string gameName)
		{		
			var asyncDelegation = new AsyncDelegation(restService);			
			asyncDelegation
				.Post("add", new { id = gameId.ToString(), name=gameName })
					.WhenFinished(() => FetchGames());
			asyncDelegation.Go();
		}
	}
}

