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

        private ModalAnimatedTransitioningType TransitioningType { get; }

        protected abstract void AnimatePresentingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController originatingController, UIViewController destinationController);
        protected abstract void AnimateDismissingInContext(IUIViewControllerContextTransitioning transitioningContext, UIViewController originatingController, UIViewController destinationController);

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            UIViewController destinationController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            UIViewController originatingController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);

            switch (TransitioningType)
            {
                case ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypeDismiss:
                    this.AnimateDismissingInContext(transitionContext, originatingController, destinationController);
                    break;
                case ModalAnimatedTransitioningType.ModalAnimatedTransitioningTypePresent:
                    this.AnimatePresentingInContext(transitionContext, originatingController, destinationController);
                    break;
            }
        }
    }
}
