using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    [CreateAssetMenu(fileName = "new Resource Piece", menuName = "Resource/Resource Piece", order = 0)]
    public class ResourcePieceSo : ScriptableObject
    {
        public GameObject prefab;
    }
}