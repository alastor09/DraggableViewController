using System;
using DraggableViewController.Core;
using Foundation;
using UIKit;

namespace DraggableView
{
    public partial class NextViewController : UIViewController
    {
        public ITransitionHandler TransitionHandler { get; set; }

        protected NextViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        partial void DonePressed(NSObject sender)
        {
            TransitionHandler.TransitionViewBack();
        }
    }
}

