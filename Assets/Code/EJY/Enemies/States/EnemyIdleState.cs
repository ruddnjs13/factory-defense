using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class EnemyIdleState : EnemyState
    {
        private EnemyAttackCompo _attackCompo;

        public EnemyIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
        }

        public override void Update()
        {
            bool inRange = _attackCompo.IsTargeting && _attackCompo.InAttackRange;
            
            if (inRange && _attackCompo.CanAttack)
            {
                _enemy.ChangeState("ATTACK");
            } 
            else if(!inRange)
            {
                _enemy.ChangeState("MOVE");
            }
        }
    }
}