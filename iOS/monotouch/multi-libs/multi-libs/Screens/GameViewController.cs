
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RestfulAdapter;

namespace multilibs
{
	public partial class GameViewController : UIViewController
	{
		private String baseUri = "http://localhost:3000/";
//		private String baseUri = "http://dry-peak-5299.herokuapp.com/";
		private RestFacilitator restFacilitator;
		private RestService restService;
		private bool shouldPool;

		private Guid _gameId;

		public GameViewController () : this(Guid.Empty){}

		public GameViewController (Guid gameId) : base("GameViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Name", "Name");
			restFacilitator = new RestFacilitator();
			restService = new RestService(restFacilitator, baseUri);
			_gameId = gameId;
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			shouldPool = true;
			PollGameData();
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			shouldPool = false;
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

		private void PollGameData ()
		{
			if(!shouldPool) 
				return;

			var asyncDelegation = new AsyncDelegation (restService);
			asyncDelegation.Get<Hash> ("gamebyid", new { id = _gameId })
				.WhenFinished (
				result =>
				{					
					JsonVisualization jsonVisualization = new JsonVisualization ();
					jsonVisualization.Parse ("root", result, 0);
					var game = new Game()
					{ 
						Id = _gameId,
						Name = result["name"].String(),
						CurrentBlackCard = result["currentBlackCard"].String(),
						IsOver = (bool) result["isOver"],
						IsStarted = (bool) result["isStarted"],
						IsReadyForScoring = (bool) result["isReadyForScoring"],
						IsReadyForReview = (bool) result["isReadyForReview"]

					};
					InvokeOnMainThread(() => {
						TextView.Text = jsonVisualization.JsonResult;
						UpdateView(game);
					});
				});			
			asyncDelegation.Go ();

			NSTimer.CreateScheduledTimer (6.0, delegate {
				PollGameData();
			});
		}

		private void UpdateView (Game game)
		{
			Title = game.Name;

			if (game.IsStarted) {
				BlackCard.Text = game.CurrentBlackCard;
			} else {
				BlackCard.Text = "waiting on round to start";
			}
		}
	}
}

