
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace HelloViews
{
	public partial class TapViewController : UIViewController
	{
		public TapViewController () : base ("TapViewController", null)
		{
			this.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites, 1);
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
			Title = string.Format("Tap ({0})", this.NavigationController.ViewControllers.Length);

			var rightButton = new UIBarButtonItem("Push",UIBarButtonItemStyle.Bordered, this, new MonoTouch.ObjCRuntime.Selector("push"));
			this.NavigationItem.RightBarButtonItem = rightButton;
		}

		[Export("push")]
		public void push ()
		{
			var new_controller = new TapViewController();
			this.NavigationController.PushViewController(new_controller, true);
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

