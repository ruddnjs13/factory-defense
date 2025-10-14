using UnityEngine;

namespace Code.SHS.Machines
{
    [CreateAssetMenu(fileName = "MachineSO", menuName = "Machine/MachineSO", order = 0)]
    public class MachineSO : ScriptableObject
    {
        public string machineName;
        public GameObject machineGhostPrefab;
        public GameObject machinePrefab;
        public int cost = 20;
    }
}