using System;
using UnityEngine;

namespace Blade.FSM
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/FSM/StateData", order = 0)]
    public class StateDataSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public string animParamName;
        
        //이 해시값은 절대로 private으로 하면 안된다. (빌드했을 때 작동 안하게 되버림.)
        public int animationHash;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animParamName);
        }
    }
}