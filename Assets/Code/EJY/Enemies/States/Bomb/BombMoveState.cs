using Code.Entities;

namespace Code.EJY.Enemies.States
{
    public class BombMoveState : EnemyMoveState
    {
        public BombMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            if(_detector.InAttackRange)
                _enemy.ChangeState("READY");
        }
    }
}