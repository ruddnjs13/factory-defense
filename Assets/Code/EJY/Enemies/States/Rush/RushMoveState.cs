using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class RushMoveState : EnemyMoveState
    {
        private const string RUSH_VFX_NAME = "Rush";
        private readonly float RUSH_DISTANCE = 8f;
        private readonly int RUSH_HASH = Animator.StringToHash("RUSH");
        private bool _isConfirm = false;
        private RushEnemyAttackCompo _rushAttackCompo;
        private EntityVFX _entityVFX;
        
        public RushMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _rushAttackCompo = _attackCompo as RushEnemyAttackCompo;
            _entityVFX = _enemy.GetCompo<EntityVFX>();
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
                _entityVFX.PlayVfx(RUSH_VFX_NAME,Vector3.zero, Quaternion.identity);
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
                _entityVFX.StopVfx(RUSH_VFX_NAME);
            }
            
            base.Exit();
        }
    }
}