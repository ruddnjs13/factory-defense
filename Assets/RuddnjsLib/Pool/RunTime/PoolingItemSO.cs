using UnityEngine;

namespace RuddnjsPool
{
    [CreateAssetMenu(menuName = "SO/Pool/Item")]    
    public class PoolingItemSO : ScriptableObject
    {
        public PoolTypeSO poolType;
        public GameObject prefab;
        public int initCount;
    }
}
