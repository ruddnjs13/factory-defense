using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies.States
{
    public class BombReadyState : EnemyState
    {
        private BombAttackCompo _attackCompo;
        private float _currentTimer;
        
        public BombReadyState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = _enemy.GetCompo<BombAttackCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _currentTimer = 0;
        }

        public override void Update()
        {
            if (_isTriggerCall)
            {
                _currentTimer += Time.deltaTime;
                if(_currentTimer >= _attackCompo.AttackInterval)
                    _enemy.ChangeState("ATTACK");
            }
        }
    }
}