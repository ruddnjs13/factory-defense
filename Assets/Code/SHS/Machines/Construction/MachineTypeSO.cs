using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines.Construction
{
    [CreateAssetMenu(fileName = "New MachineTypeSO", menuName = "Machine/MachineTypeSO", order = 0)]
    public class MachineTypeSO : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        public List<MachineSO> machines = new List<MachineSO>();
    }
}