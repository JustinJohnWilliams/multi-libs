using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RestfulAdapter;

namespace HelloRest
{
	public partial class HelloRestViewController : UIViewController
	{
		private String baseUri = "http://localhost:3000/";
		private RestFacilitator restFacilitator;
		private RestService restService;
		private List<string> peeps;
		private UITableView table;

		public HelloRestViewController () : base ("HelloRestViewController", null)
		{
			restFacilitator = new RestFacilitator();
			restService = new RestService(restFacilitator, baseUri);
			peeps = new List<string>();
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
			Title = "Peeps";
			this.View.BackgroundColor = UIColor.White;

			var rightButton = new UIBarButtonItem("+",UIBarButtonItemStyle.Bordered, this, new MonoTouch.ObjCRuntime.Selector("CreatePeep"));
			this.NavigationItem.RightBarButtonItem = rightButton;

			var asyncDelegation = new AsyncDelegation(restService);

//			asyncDelegation.Get<Hash>("search.json", new { q = "#haiku" })
//				.WhenFinished(
//					result =>
//					{
//					List<string> tweets = new List<string>();
//					textBlockTweets.Text = "";
//					foreach (var tweetObject in result["results"].ToHashes())
//					{
//						textBlockTweets.Text += HttpUtility.HtmlDecode(tweetObject["text"].ToString()) + Environment.NewLine + Environment.NewLine;
//					}
//				});
			
			asyncDelegation.Go();

			table = new UITableView(this.View.Bounds);
			this.View.AddSubview(table);
			
			peeps = new List<String> () { "A", "B", "C", "D", "E", "F" };
			table.Source = new TableSource(peeps);
		}

		[Export("CreatePeep")]
		public void RightButtonPush ()
		{
			peeps.Add("G");
			table.ReloadData();
			var asyncDelegation = new AsyncDelegation(restService);
//			asyncDelegation.Post("readyForNextRound", new {gameId = _gameId, playerId = Application.PlayerId})
//				.WhenFinished(()=> {
//
//				});
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

