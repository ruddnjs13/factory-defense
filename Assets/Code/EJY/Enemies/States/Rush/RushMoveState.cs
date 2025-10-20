using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class RushMoveState : EnemyMoveState
    {
        private readonly float RUSH_DISTANCE = 8f;
        private bool _isConfirm = false;
        private readonly int RUSH_HASH = Animator.StringToHash("RUSH");
        private RushEnemyAttackCompo _rushAttackCompo;
        
        public RushMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _rushAttackCompo = _attackCompo as RushEnemyAttackCompo;
        }

        public override void Enter()
        {
            base.Enter();

            Transform target = _detector.IsTargeting ? _detector.CurrentTarget.Value : _enemy.TargetTrm;
            float distance = Vector3.Distance(target.position, _entity.transform.position);

            if (distance > RUSH_DISTANCE)
            {
                _movement.SpeedMultiplier = 2f;
                _isConfirm = true;
            }
            else
            {
                _movement.SpeedMultiplier = 1f;
                _isConfirm = false;
            }
            
            _entityAnimator.SetParam(RUSH_HASH, _isConfirm);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            if (_isConfirm)
            {
                _rushAttackCompo.RushEnd();
                _movement.SpeedMultiplier = 1f;
                _entityAnimator.SetParam(RUSH_HASH, false);
            }
            
            base.Exit();
        }
    }
}