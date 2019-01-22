using UIKit;

namespace DraggableViewController.Core
{
    public enum ModalAnimatedTransitioningType
    {
        ModalAnimatedTransitioningTypePresent = 0,
        ModalAnimatedTransitioningTypeDismiss = 1
    }

    public abstract class BaseAnimator
    {
        public ModalAnimatedTransitioningType TransitioningType { get; set; }

        internal abstract void AnimatePresentingInContext(UIViewControllerContextTransitioning transitioningContext, UIViewController destinationViewController, UIViewController startingViewController);
        internal abstract void AnimateDismissingInContext(UIViewControllerContextTransitioning transitioningContext, UIViewController destinationViewController, UIViewController startingViewController);

        void AnimateTransition(UIViewControllerContextTransitioning transitioningContext)
        {
            UIViewController to = transitioningContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            UIViewController from = transitioningContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);

            switch (TransitioningType)
            {
                case ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypeDismiss:
                    break;
                case ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypePresent:
                    break;
                default:
                    break;
            }
        }
    }
}
