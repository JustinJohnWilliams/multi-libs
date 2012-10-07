// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace multilibs
{
	[Register ("HomeViewController")]
	partial class HomeViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView GamesTable { get; set; }

		[Action ("CreateClicked:")]
		partial void CreateClicked (MonoTouch.Foundation.NSObject sender);

		[Action ("RefreshClicked:")]
		partial void RefreshClicked (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (GamesTable != null) {
				GamesTable.Dispose ();
				GamesTable = null;
			}
		}
	}
}
