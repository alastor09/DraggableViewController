using System;
using CoreGraphics;
using UIKit;

namespace DraggableViewController.Core
{
    public class MiniToLargeViewAnimator : BaseAnimator
    {
        static double AnimationDuration = .4f;

        UIView FakeView { get; }

        internal double InitialY { get; private set; }

        public MiniToLargeViewAnimator(UIView fakeView, double initialY)
        {
            this.FakeView = fakeView;
            this.InitialY = initialY;
        }

        double TransitionDuration(UIViewControllerContextTransitioning transitioningContext)
            => AnimationDuration;

        internal override void AnimateDismissingInContext(UIViewControllerContextTransitioning transitioningContext, UIViewController toVC, UIViewController fromVC)
        {
            var fromVCrect = transitioningContext.GetInitialFrameForViewController(fromVC);
            fromVCrect.Y = (nfloat)(fromVCrect.Size.Height - this.InitialY);

            UIView imageView = this.FakeView;
            fromVC.View.AddSubview(imageView);

            UIView container = transitioningContext.ContainerView;
            container.AddSubview(toVC.View);
            container.AddSubview(fromVC.View);
            imageView.Alpha = 0.0f;

            UIView.Animate(AnimationDuration, () =>
                            {
                                fromVC.View.Frame = fromVCrect;
                                imageView.Alpha = 1.0f;
                            },
                            () =>
                            {
                                imageView.RemoveFromSuperview();
                                if (transitioningContext.TransitionWasCancelled)
                                {
                                    transitioningContext.CompleteTransition(false);
                                    toVC.View.RemoveFromSuperview();
                                }
                                else
                                {
                                    transitioningContext.CompleteTransition(true);
                                }
                            });
        }

        internal override void AnimatePresentingInContext(UIViewControllerContextTransitioning transitioningContext, UIViewController toVC, UIViewController fromVC)
        {
            CGRect fromVCrect = transitioningContext.GetInitialFrameForViewController(fromVC);
            CGRect toVCRect = fromVCrect;
            toVCRect.Y = (nfloat)(toVCRect.Size.Height - this.InitialY);

            toVC.View.Frame = toVCRect;
            UIView container = transitioningContext.ContainerView;
            UIView imageView = this.FakeView;
            toVC.View.AddSubview(imageView);

            container.AddSubview(fromVC.View);
            container.AddSubview(toVC.View);

            UIView.Animate(AnimationDuration, () =>
            {
                toVC.View.Frame = fromVCrect;
                imageView.Alpha = 0.0f;
            },
                            () =>
                            {
                                imageView.RemoveFromSuperview();
                                if (transitioningContext.TransitionWasCancelled)
                                {
                                    transitioningContext.CompleteTransition(false);
                                }
                                else
                                {
                                    transitioningContext.CompleteTransition(true);
                                }
                            });
        }
    }
}
