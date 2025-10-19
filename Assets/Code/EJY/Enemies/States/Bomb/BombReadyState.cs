using Code.Entities;

namespace Code.EJY.Enemies.States
{
    public class BombReadyState : EnemyState
    {
        public BombReadyState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            if(_isTriggerCall)
                _enemy.ChangeState("ATTACK");
        }
    }
}