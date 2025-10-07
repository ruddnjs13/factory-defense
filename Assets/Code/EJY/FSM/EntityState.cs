using Code.Entities;

namespace Code.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected int _animationHash;
        protected EntityAnimator _entityAnimator;
        protected EntityAnimatorTrigger _animatorTrigger;
        protected bool _isTriggerCall;

        public EntityState(Entity entity, int animationHash)
        {
            _entity = entity;
            _animationHash = animationHash;
            _entityAnimator = entity.GetCompo<EntityAnimator>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
        }

        public virtual void Enter()
        {
            _entityAnimator.SetParam(_animationHash, true);
            _isTriggerCall = false;
            _animatorTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
        }

        public virtual void Update() { }

        public virtual void Exit()
        {
            _entityAnimator.SetParam(_animationHash, false);
            _animatorTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
        }

        public virtual void AnimationEndTrigger() => _isTriggerCall = true;
    }
}