using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace HelloWorld
{
	public partial class HelloWorldViewController : UIViewController
	{
		private int count;

		public HelloWorldViewController () : base ("HelloWorldViewController", null)
		{
			count = 0;
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

			var countLabel = new UILabel();
			countLabel.Text = "0";
			countLabel.BackgroundColor = UIColor.Black;
			countLabel.TextColor = UIColor.White;
			countLabel.Frame = new RectangleF(20f, 260f, 280f, 53f);
			countLabel.TextAlignment = UITextAlignment.Center;

			var countButton = UIButton.FromType(UIButtonType.RoundedRect);
			countButton.Frame = new RectangleF(80f, 200f, 150f, 44f);
			countButton.SetTitle("Count", UIControlState.Normal);
			countButton.TouchUpInside += (sender, e) => {
				count++;
				countLabel.Text = count.ToString();
			};

			this.View.Add(countLabel);
			this.View.Add(countButton);
		}

		partial void sayHelloClicked (NSObject sender)
		{
			HelloLabel.Text = "Hello, iPhone";
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

