using System;
using UIKit;

namespace DraggableViewController.Core
{
    public class MiniToLargeViewInteractive : UIPercentDrivenInteractiveTransition
    {
        readonly UIViewController ViewController;
        readonly UIViewController PresentViewController;
        readonly UIPanGestureRecognizer Pan;

        public MiniToLargeViewInteractive(UIViewController viewController, UIViewController presentViewController, UIView view)
        {
            ViewController = viewController;
            PresentViewController = presentViewController;
            Pan = new UIPanGestureRecognizer((UIPanGestureRecognizer obj) => OnPan(obj));
            view.AddGestureRecognizer(Pan);
        }

        void OnPan(UIPanGestureRecognizer panGestureRecognizer)
        {
            var translation = panGestureRecognizer.TranslationInView(panGestureRecognizer.View.Superview);

            switch (panGestureRecognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    {
                        if (this.PresentViewController == null)
                        {
                            this.ViewController.DismissViewController(true, null);
                        }
                        else
                        {
                            this.ViewController.PresentViewController(this.PresentViewController, true, null);
                        }
                    }
                    break;
                case UIGestureRecognizerState.Changed:
                    {
                        double ScreenHeight = UIScreen.MainScreen.Bounds.Size.Height - 50.0f;
                        double DragAmount = this.PresentViewController == null ? ScreenHeight : -ScreenHeight;
                        double Threshold = 0.3;

                        double percent = translation.Y / DragAmount;
                        percent = Math.Max(percent, 0.0f);
                        percent = Math.Min(percent, 1.0f);
                        this.UpdateInteractiveTransition((nfloat)percent);
                        this.ShouldComplete = percent > Threshold;
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
        }

        double CompletitionSpeed
            => 1.0 - this.PercentComplete;

        bool ShouldComplete;
    }
}
