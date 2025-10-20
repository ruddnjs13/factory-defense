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
                var target = _detector.CurrentTarget.Value.position;
                _movement.LookAtTarget(target);

                float angle = Quaternion.Angle(
                    Quaternion.LookRotation(target - _enemy.transform.position),
                    _enemy.transform.rotation
                );

                if (angle <= ANGLETHRESHOLD) break;
                yield return null;
            }
            
            Debug.Log("회전 끝");
            _animator.SetAnimator(true);
        }

        public override void Update()
        {
            if(_isTriggerCall)
                _enemy.ChangeState("IDLE");
        }
        
        public override void Exit()
        {
            base.Exit();
            _attackCompo.Attack();
        }
    }
}