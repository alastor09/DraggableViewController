// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DraggableView
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIView BottomView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint BottomViewHeightConstraint { get; set; }

		[Action ("DummyButtonTapped:")]
		partial void DummyButtonTapped (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BottomViewHeightConstraint != null) {
				BottomViewHeightConstraint.Dispose ();
				BottomViewHeightConstraint = null;
			}

			if (BottomView != null) {
				BottomView.Dispose ();
				BottomView = null;
			}
		}
	}
}
