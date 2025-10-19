using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    [CreateAssetMenu(fileName = "MachineSO", menuName = "Machine/MachineSO", order = 0)]
    public class MachineSO : ScriptableObject
    {
        public string machineName;
        public GameObject machinePreviewPrefab;
        public GameObject machinePrefab;
        public Vector2Int size = Vector2Int.one;
        public Vector2Int offset = Vector2Int.zero;
        public int cost = 20;
    }
}