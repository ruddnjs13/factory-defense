using System.Collections.Generic;
using UnityEngine;

namespace RuddnjsPool
{
    [CreateAssetMenu(menuName = "SO/Pool/Manager")]
    public class PoolManagerSO : ScriptableObject
    {
        public List<PoolingItemSO> poolingItemList;
        [SerializeField] private Transform _rootTrm;

        private Dictionary<PoolTypeSO, Pool> _pools;

        public void InitializePool(Transform root)
        {
            _rootTrm = root;
            _pools = new Dictionary<PoolTypeSO, Pool>();

            foreach (var item in poolingItemList)
            {
                var poolable = item.prefab.GetComponent<IPoolable>();
                Debug.Assert(poolable != null, $"Prefab must have IPoolable component {item.prefab.name}");

                var pool = new Pool(poolable, _rootTrm, item.initCount, item.poolType);
                _pools.Add(item.poolType, pool);
            }
        }

        public IPoolable Pop(PoolTypeSO type)
        {
            if (_pools.TryGetValue(type, out Pool pool))
            {
                return pool.Pop();
            }

            return null;
        }

        public void Push(IPoolable item)
        {
            if (_pools.TryGetValue(item.PoolType, out Pool pool))
            {
                pool.Push(item);
            }
        }
    }
}