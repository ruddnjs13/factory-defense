using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class EnemyIdleState : EnemyState
    {
        private EnemyAttackCompo _attackCompo;
        private TargetDetector _detector;
        
        public EnemyIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
            _detector = entity.GetCompo<TargetDetector>();
        }

        public override void Update()
        {
            bool inRange = _detector.IsTargeting.Value && _detector.InAttackRange;
            
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