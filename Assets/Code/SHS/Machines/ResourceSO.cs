using UnityEngine;

namespace Code.SHS.Machines
{
    [CreateAssetMenu(fileName = "new ResourceSO", menuName = "ResourceSO", order = 0)]
    public class ResourceSO : ScriptableObject
    {
        public string resourceName;
        public GameObject prefab;
    }
}