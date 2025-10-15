using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    [CreateAssetMenu(fileName = "new ResourceSO", menuName = "Resource/ShapeResourceSO", order = 0)]
    public class ShapeResourceSO : ScriptableObject
    {
        [field: SerializeField] public ResourcePieceSo[] ResourcePieces { get; set; } = new ResourcePieceSo[8];
    }
}