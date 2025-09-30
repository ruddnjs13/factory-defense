using RuddnjsLib.Dependencies;
using UnityEngine;
using UnityEngine.Serialization;

namespace RuddnjsPool.RuddnjsLib.Pool.RunTime
{
    public class PoolManagerMono : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private PoolManagerSO poolManager;

        private void Awake()
        {
            poolManager.InitializePool(transform);
        }
        
        public T Pop<T>(PoolingItemSO item) where T : IPoolable
        {
            return (T)poolManager.Pop(item.poolType);
        }

        public void Push(IPoolable target)
        {
            poolManager.Push(target);
        }
    }
}
