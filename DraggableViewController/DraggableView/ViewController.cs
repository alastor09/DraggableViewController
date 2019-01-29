using System;
using Foundation;
using UIKit;
using DraggableViewController.Core;

namespace DraggableView
{
    public partial class ViewController : UIViewController, IUIViewControllerTransitioningDelegate, ITransitionHandler
    {
        public bool DisableInteractiveTransitioning;
        MiniToLargeViewInteractive PresentInteractor;
        MiniToLargeViewInteractive DismissInteractor;
        NextViewController NextViewController;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            BottomView.TranslatesAutoresizingMaskIntoConstraints = false;

            NextViewController = (NextViewController)this.Storyboard.InstantiateViewController(nameof(NextViewController));
            NextViewController.TransitionHandler = this;
            NextViewController.TransitioningDelegate = this;
            NextViewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

            this.PresentInteractor = new MiniToLargeViewInteractive(this, NextViewController, BottomView);
            this.DismissInteractor = new MiniToLargeViewInteractive(NextViewController, null, NextViewController.View);
        }

        public void TransitionViewBack()
        {
            this.DisableInteractiveTransitioning = true;
            this.DismissViewController(true, () => this.DisableInteractiveTransitioning = false);
        }

        partial void DummyButtonTapped(NSObject sender)
        {
            this.DisableInteractiveTransitioning = true;
            this.PresentViewController(NextViewController, true, () => { this.DisableInteractiveTransitioning = false; });
        }

        [Export("animationControllerForDismissedController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        {
            return new MiniToLargeViewAnimator(BottomViewHeightConstraint.Constant, ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypeDismiss, BottomView);
        }

        [Export("animationControllerForPresentedController:presentingController:sourceController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            return new MiniToLargeViewAnimator(BottomViewHeightConstraint.Constant, ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypePresent, BottomView);
        }

        [Export("interactionControllerForDismissal:")]
        public IUIViewControllerInteractiveTransitioning GetInteractionControllerForDismissal(IUIViewControllerAnimatedTransitioning animator)
        {
            return this.DisableInteractiveTransitioning? null: this.DismissInteractor;
        }

        [Export("interactionControllerForPresentation:")]
        public IUIViewControllerInteractiveTransitioning GetInteractionControllerForPresentation(IUIViewControllerAnimatedTransitioning animator)
        {
            return this.DisableInteractiveTransitioning ? null: this.PresentInteractor;
        }
    }
}
