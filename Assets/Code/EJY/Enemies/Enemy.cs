using Code.Entities;
using RuddnjsPool;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public abstract class Enemy : Entity, IPoolable
    {
        [field: SerializeField] public Transform TargetTrm { get; private set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }
        
        public void SetTarget(Transform targetTrm) => TargetTrm = targetTrm;
        public GameObject GameObject => gameObject;
        private Pool _myPool;
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            
        }

        public void PushEnemyInPool() => _myPool.Push(this);
    }
}