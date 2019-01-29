using System;
using CoreGraphics;
using UIKit;

namespace DraggableViewController.Core
{
    public class MiniToLargeViewAnimator : BaseAnimator
    {
        static double AnimationDuration = .4f;

        UIImage DummyImage { get; }

        internal double InitialY { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DraggableViewController.Core.MiniToLargeViewAnimator"/> class.
        /// </summary>
        /// <param name="initialY">Starting Y Position of introductoryView</param>
        /// <param name="transitionType">Type Of Transition</param>
        /// <param name="introductoryView">View which is Displayed as a Introduction</param>
        public MiniToLargeViewAnimator(double initialY, ModalAnimatedTransitioningType transitionType, UIView introductoryView) : base(transitionType)
        {
            this.InitialY = initialY;
            DummyImage = CreateImageFromView(introductoryView);
        }

        UIImage CreateImageFromView(UIView briefView)
        {
            var renderer = new UIGraphicsImageRenderer(briefView.Bounds, format: UIGraphicsImageRendererFormat.DefaultFormat);
            return renderer.CreateImage(arg => briefView.Layer.RenderInContext(arg.CGContext));
        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
            => AnimationDuration;

        protected override void AnimateDismissingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController originatingController, UIViewController destinationController)
        {
            var fromVCrect = transitioningContext.GetInitialFrameForViewController(originatingController);
            fromVCrect.Y = (nfloat)(fromVCrect.Size.Height - this.InitialY);

            UIView imageView = new UIImageView(DummyImage);
            imageView.Alpha = 0.0f;
            originatingController.View.AddSubview(imageView);

            UIView container = transitioningContext.ContainerView;
            container.AddSubview(destinationController.View);
            container.AddSubview(originatingController.View);

            UIView.Animate(AnimationDuration, (() =>
                            {
                                originatingController.View.Frame = fromVCrect;
                                imageView.Alpha = 1.0f;
                            }),
                            (() =>
                            {
                                imageView.RemoveFromSuperview();
                                if (transitioningContext.TransitionWasCancelled)
                                {
                                    transitioningContext.CompleteTransition(false);
                                    originatingController.View.RemoveFromSuperview();
                                }
                                else
                                {
                                    transitioningContext.CompleteTransition(true);
                                }
                            }));
        }

        protected override void AnimatePresentingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController originatingController, UIViewController destinationController)
        {
            CGRect fromVCrect = transitioningContext.GetInitialFrameForViewController(originatingController);
            CGRect toVCRect = fromVCrect;
            toVCRect.Y = (nfloat)(toVCRect.Size.Height - this.InitialY);

            destinationController.View.Frame = toVCRect;
            UIView container = transitioningContext.ContainerView;
            UIView imageView = new UIImageView(DummyImage);
            destinationController.View.AddSubview(imageView);

            container.AddSubview(originatingController.View);
            container.AddSubview(destinationController.View);

            UIView.Animate(AnimationDuration, () =>
            {
                destinationController.View.Frame = fromVCrect;
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
