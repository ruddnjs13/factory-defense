using Code.Combat;
using RuddnjsPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Feedbacks
{
    public class HitImpact : MonoBehaviour
    {
        [field: SerializeField] public DamageType AllowedDamageType;

        [field: SerializeField] public PoolingItemSO PoolingItem { get; private set; }
    }
}