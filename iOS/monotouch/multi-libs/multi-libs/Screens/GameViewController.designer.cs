// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace multilibs
{
	[Register ("GameViewController")]
	partial class GameViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextView TextView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel BlackCard { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TextView != null) {
				TextView.Dispose ();
				TextView = null;
			}

			if (BlackCard != null) {
				BlackCard.Dispose ();
				BlackCard = null;
			}
		}
	}
}
