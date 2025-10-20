using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Worlds
{
    [CreateAssetMenu(fileName = "new ResourceGround SO", menuName = "Ground/ResourceGround", order = 0)]
    public class ResourceGroundSO : GroundSO
    {
        [field: SerializeField] public ShapeResourceSO ResourceSO { get; private set; }
    }
}