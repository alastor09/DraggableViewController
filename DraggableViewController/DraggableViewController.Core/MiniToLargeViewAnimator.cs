using System;
using CoreGraphics;
using UIKit;

namespace DraggableViewController.Core
{
    public class MiniToLargeViewAnimator : BaseAnimator
    {
        static double AnimationDuration = .4f;

        UIImage FakeImage { get; }

        internal double InitialY { get; private set; }

        public MiniToLargeViewAnimator(double initialY, ModalAnimatedTransitioningType transitionType, UIView bottomView) : base(transitionType)
        {
            this.InitialY = initialY;
            FakeImage = SnapshotImage(bottomView);
        }

        UIImage SnapshotImage(UIView fakeView)
        {
            var renderer = new UIGraphicsImageRenderer(fakeView.Bounds, format: UIGraphicsImageRendererFormat.DefaultFormat);
            return renderer.CreateImage(arg => fakeView.Layer.RenderInContext(arg.CGContext));
        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
            => AnimationDuration;

        internal override void AnimateDismissingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController fromVC, UIViewController toVC)
        {
            var fromVCrect = transitioningContext.GetInitialFrameForViewController(fromVC);
            fromVCrect.Y = (nfloat)(fromVCrect.Size.Height - this.InitialY);

            UIView imageView = new UIImageView(FakeImage);
            imageView.Alpha = 0.0f;
            fromVC.View.AddSubview(imageView);

            UIView container = transitioningContext.ContainerView;
            container.AddSubview(toVC.View);
            container.AddSubview(fromVC.View);

            UIView.Animate(AnimationDuration, (Action)(() =>
                            {
                                fromVC.View.Frame = fromVCrect;
                                imageView.Alpha = 1.0f;
                            }),
(Action)(() =>
                            {
                                imageView.RemoveFromSuperview();
                                if (transitioningContext.TransitionWasCancelled)
                                {
                                    transitioningContext.CompleteTransition(false);
                                    fromVC.View.RemoveFromSuperview();
                                }
                                else
                                {
                                    transitioningContext.CompleteTransition(true);
                                }
                            }));
        }

        internal override void AnimatePresentingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController fromVC, UIViewController toVC)
        {
            CGRect fromVCrect = transitioningContext.GetInitialFrameForViewController(fromVC);
            CGRect toVCRect = fromVCrect;
            toVCRect.Y = (nfloat)(toVCRect.Size.Height - this.InitialY);

            toVC.View.Frame = toVCRect;
            UIView container = transitioningContext.ContainerView;
            UIView imageView = new UIImageView(FakeImage);
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
