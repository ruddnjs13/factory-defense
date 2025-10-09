using Code.Enemies;
using Code.Entities;

namespace Code.EJY.Enemies.States
{
    public class EnemyAttackState : EnemyState
    {
        private EnemyAttackCompo _attackCompo;
        
        public EnemyAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
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