using Code.Enemies;
using Code.Entities;
using Code.FSM;

namespace Code.EJY.Enemies.States
{
    public abstract class EnemyState : EntityState
    {
        protected FSMEnemy _enemy;
        
        public EnemyState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _enemy = entity as FSMEnemy;
        }
    }
}