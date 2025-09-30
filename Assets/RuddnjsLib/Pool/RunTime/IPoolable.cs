using UnityEngine;

namespace RuddnjsPool
{
    public interface IPoolable
    {
        public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject { get; }
        public void SetUpPool(Pool pool);
        public void ResetItem();
    }
}
