using UIKit;

namespace DraggableViewController.Core
{
    public enum ModalAnimatedTransitioningType
    {
        ModalAnimatedTransitioningTypePresent = 0,
        ModalAnimatedTransitioningTypeDismiss = 1
    }

    public abstract class BaseAnimator : UIViewControllerAnimatedTransitioning
    {
        protected BaseAnimator(ModalAnimatedTransitioningType transitioningType)
        {
            TransitioningType = transitioningType;
        }

        public ModalAnimatedTransitioningType TransitioningType { get; }

        internal abstract void AnimatePresentingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController fromVC, UIViewController toVC);
        internal abstract void AnimateDismissingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController fromVC, UIViewController toVC);

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            UIViewController toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            UIViewController fromVC = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);

            switch (TransitioningType)
            {
                case ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypeDismiss:
                    this.AnimateDismissingInContext(transitionContext, fromVC, toVC);
                    break;
                case ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypePresent:
                    this.AnimatePresentingInContext(transitionContext, fromVC, toVC);
                    break;
                default:
                    break;
            }
        }
    }
}
