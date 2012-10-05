
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
			var asyncDelegation = new AsyncDelegation(restService);
			asyncDelegation.Get<Hash>("gamebyid", new { id = _gameId })
				.WhenFinished(
					result =>
					{
					Title = result["name"].ToString();
				});			
			asyncDelegation.Go();
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
	}
}

