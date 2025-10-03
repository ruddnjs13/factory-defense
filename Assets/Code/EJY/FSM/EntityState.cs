using Blade.Entities;

namespace Blade.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected int _animationHash;
        protected EntityAnimator _entityAnimator;
        protected EntityAnimatorTrigger _animatorTrigger; //1
        protected bool _isTriggerCall;

        public EntityState(Entity entity, int animationHash)
        {
            _entity = entity;
            _animationHash = animationHash;
            _entityAnimator = entity.GetCompo<EntityAnimator>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>(); //2
        }

        public virtual void Enter()
        {
            _entityAnimator.SetParam(_animationHash, true);
            _isTriggerCall = false;
            _animatorTrigger.OnAnimationEndTrigger += AnimationEndTrigger; //3
        }

        public virtual void Update() { }

        public virtual void Exit()
        {
            _entityAnimator.SetParam(_animationHash, false);
            _animatorTrigger.OnAnimationEndTrigger -= AnimationEndTrigger; //4
        }

        public virtual void AnimationEndTrigger() => _isTriggerCall = true;
    }
}