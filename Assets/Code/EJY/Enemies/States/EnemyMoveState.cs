using Code.Enemies;
using Code.Entities;

namespace Code.EJY.Enemies.States
{
    public class EnemyMoveState : EnemyState
    {
        private EnemyAttackCompo _attackCompo;
        private TargetDetector _detector;
        private NavMovement _movement;
        
        public EnemyMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
            _detector = entity.GetCompo<TargetDetector>();
            _movement = entity.GetCompo<NavMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.SetStop(false);
        }

        public override void Update()
        {
            if (_detector.IsTargeting && _attackCompo.InAttackRange(_detector.CurrentTarget.position))
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