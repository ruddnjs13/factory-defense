using Code.Enemies;
using Code.FSM;
using Code.EJY.Enemies;
using UnityEngine;

namespace Code.Enemies
{
    public class FSMEnemy : Enemy
    {
        [SerializeField] private StateDataSO[] states;
        private EntityStateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
        }

        public override void Init(Transform targetTrm)
        {
            base.Init(targetTrm);
            ChangeState("MOVE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void ChangeState(string newState) => _stateMachine.ChangeState(newState);

        public override void SetDead()
        {
            base.SetDead();
            ChangeState("DEAD");
        }
    }
}