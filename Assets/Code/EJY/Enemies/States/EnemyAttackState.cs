using Code.Enemies;
using Code.Entities;
using UnityEngine.AI;

namespace Code.EJY.Enemies.States
{
    public class EnemyAttackState : EnemyState
    {
        private EnemyAttackCompo _attackCompo;
        private NavMovement _movement;
        
        public EnemyAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
            _movement = entity.GetCompo<NavMovement>();
        }

        public override void Enter()
        {
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