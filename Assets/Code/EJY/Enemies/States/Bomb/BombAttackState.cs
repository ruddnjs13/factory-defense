using Code.Entities;

namespace Code.EJY.Enemies.States
{
    public class BombAttackState : EnemyState
    {
        public BombAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
    }
}