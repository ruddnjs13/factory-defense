using System;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float  moveSpeed;
        [SerializeField] private float  lifeTime;
        private float _timer = 0;

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _myPool.Push(this);
            }
        }

        public void InitProjectile(Vector3 direction)
        {
            _timer = lifeTime;
            rigidbody.linearVelocity = direction * moveSpeed;
            transform.forward = direction;
        }

        #region Pool
        [field:SerializeField] public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject  => gameObject;

        private Pool _myPool;
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            _timer = lifeTime;
        }
        #endregion
    }
}