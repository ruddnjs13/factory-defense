using Code.Entities;

namespace Code.EJY.Enemies.States
{
    public class EnemyDeadState : EnemyState
    {
        public EnemyDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
    }
}