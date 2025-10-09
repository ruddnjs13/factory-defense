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

        private void Start()
        {
            _stateMachine = new EntityStateMachine(this, states);
            ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void ChangeState(string newState) => _stateMachine.ChangeState(newState);
    }
}