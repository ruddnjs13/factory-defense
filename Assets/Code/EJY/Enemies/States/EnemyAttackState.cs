using System.Collections;
using Code.Enemies;
using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class EnemyAttackState : EnemyState
    {
        private EnemyAttackCompo _attackCompo;
        private NavMovement _movement;
        private TargetDetector _detector;

        private const float ANGLETHRESHOLD = 5f;

        public EnemyAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
            _movement = entity.GetCompo<NavMovement>();
            _detector = entity.GetCompo<TargetDetector>();
        }

        public override void Enter()
        {
            Quaternion targetRot = _movement.LookAtTarget(_detector.CurrentTarget.Value.position);
            while (Quaternion.Angle(_enemy.transform.rotation, targetRot) > ANGLETHRESHOLD)
            {
                targetRot = _movement.LookAtTarget(_detector.CurrentTarget.Value.position);
            }
            base.Enter();
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