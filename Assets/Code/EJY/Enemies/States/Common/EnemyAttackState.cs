using System.Collections;
using Code.Enemies;
using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class EnemyAttackState : EnemyState
    {
        private EntityAnimator _animator;
        private EnemyAttackCompo _attackCompo;
        private NavMovement _movement;
        private TargetDetector _detector;
        
        private const float ANGLETHRESHOLD = 5f;

        public EnemyAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _animator = entity.GetCompo<EntityAnimator>();
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
            _movement = entity.GetCompo<NavMovement>();
            _detector = entity.GetCompo<TargetDetector>();
        }

        public override void Enter()
        {
            base.Enter();
            _enemy.StartCoroutine(RotateToTarget());
        } 

        private IEnumerator RotateToTarget()
        {
            _animator.SetAnimator(false);
            while (true)
            {
                if (_detector.CurrentTarget.Value == null) break;
                
                var target = _detector.CurrentTarget.Value.position;
                target.y = 0;
                _movement.LookAtTarget(target);
                
                float angle = Quaternion.Angle(
                    Quaternion.LookRotation(target - _enemy.transform.position),
                    _enemy.transform.rotation
                );

                if (angle <= ANGLETHRESHOLD) break;
                yield return null;
            }
            
            _animator.SetAnimator(true);
        }

        public override void Update()
        {
            if(_isTriggerCall)
                _enemy.ChangeState("IDLE");
        }
        
        public override void Exit()
        {
            if(!_animator.IsActive())
                _animator.SetAnimator(true);
            _attackCompo.Attack();
            base.Exit();
        }
    }
}