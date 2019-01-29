using System;
using UIKit;

namespace DraggableViewController.Core
{
    public class MiniToLargeViewInteractive : UIPercentDrivenInteractiveTransition
    {
        readonly UIViewController CurrentViewController;
        readonly UIViewController DestinationViewController;
        readonly UIPanGestureRecognizer Pan;

        public MiniToLargeViewInteractive(UIViewController currentViewcontroller, UIViewController destinationViewController, UIView gestureView)
        {
            CurrentViewController = currentViewcontroller;
            DestinationViewController = destinationViewController;
            Pan = new UIPanGestureRecognizer((UIPanGestureRecognizer obj) => OnPan(obj));
            gestureView.AddGestureRecognizer(Pan);
        }

        void OnPan(UIPanGestureRecognizer panGestureRecognizer)
        {
            var translation = panGestureRecognizer.TranslationInView(panGestureRecognizer.View.Superview);
            double ScreenHeight = UIScreen.MainScreen.Bounds.Size.Height - 50.0f;
            double DragAmount = this.DestinationViewController != null ? -ScreenHeight : ScreenHeight;
            double Threshold = 0.3;
            //Represents the difference between progress that is required to trigger the completion of the transition.
            double automaticOverrideThreshold = 0.03;

            double percent = translation.Y / DragAmount;
            percent = Math.Max(percent, 0.0f);
            percent = Math.Min(percent, 1.0f);

            switch (panGestureRecognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    {
                        if (this.DestinationViewController != null)
                        {
                            this.CurrentViewController.PresentViewController(this.DestinationViewController, true, null);
                        }
                        else
                        {
                            this.CurrentViewController.DismissViewController(true, null);
                        }
                    }
                    break;
                case UIGestureRecognizerState.Changed:
                    {
                        if (LastProgress > percent)
                        {
                            this.ShouldComplete = false;
                        }
                        else if(percent > LastProgress + automaticOverrideThreshold)
                        {
                            this.ShouldComplete = true;
                        }
                        else
                        {
                            this.ShouldComplete = percent > Threshold;
                        }
                        this.UpdateInteractiveTransition((nfloat)percent);
                    }
                    break;
                case UIGestureRecognizerState.Ended:
                case UIGestureRecognizerState.Cancelled:
                    {
                        if (panGestureRecognizer.State == UIGestureRecognizerState.Cancelled || !this.ShouldComplete)
                        {
                            this.CancelInteractiveTransition();
                        }
                        else
                        {
                            this.FinishInteractiveTransition();
                        }
                    }
                    break;
            }
            LastProgress = percent;
        }

        double CompletitionSpeed
            => 1.0 - this.PercentComplete;

        double LastProgress;
        bool ShouldComplete;
    }
}
