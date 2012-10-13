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



			table = new UITableView(this.View.Bounds);
			this.View.AddSubview(table);

			table.Source = new TableSource(peeps);
			PullData();
		}

		void PullData()
		{
			var asyncDelegation = new AsyncDelegation(restService);
			
			asyncDelegation.Get<Hashes>("list", new { })
				.WhenFinished(
					result =>
					{
					peeps.Clear();
					foreach (var peep in result)
					{
						peeps.Add(peep["Name"].ToString());
					}
					table.ReloadData();
				});
			
			asyncDelegation.Go();

		}

		[Export("CreatePeep")]
		public void RightButtonPush ()
		{
			var asyncDelegation = new AsyncDelegation(restService);
			asyncDelegation.Post("add", new {Name="MonoTouch Name" })
				.WhenFinished(()=> {
					PullData();
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

