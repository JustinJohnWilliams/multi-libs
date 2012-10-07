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
		MonoTouch.UIKit.UITableView WhiteCardTable { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel BlackCard { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PointsLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (WhiteCardTable != null) {
				WhiteCardTable.Dispose ();
				WhiteCardTable = null;
			}

			if (BlackCard != null) {
				BlackCard.Dispose ();
				BlackCard = null;
			}

			if (PointsLabel != null) {
				PointsLabel.Dispose ();
				PointsLabel = null;
			}
		}
	}
}
