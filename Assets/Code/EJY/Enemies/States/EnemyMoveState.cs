using Code.Enemies;
using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class EnemyMoveState : EnemyState
    {
        private EnemyAttackCompo _attackCompo;
        private NavMovement _movement;
        
        public EnemyMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
            _movement = entity.GetCompo<NavMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.SetStop(false);
        }

        public override void Update()
        {
            if (_attackCompo.IsTargeting && _attackCompo.InAttackRange)
            {
                _enemy.ChangeState("ATTACK");
            }
        }

        public override void Exit()
        {
            _movement.SetStop(true);
            base.Exit();
        }
    }
}