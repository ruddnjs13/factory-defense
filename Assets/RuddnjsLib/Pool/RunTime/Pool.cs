using System.Collections.Generic;
using UnityEngine;

namespace RuddnjsPool
{
    public class Pool
    {
        private Stack<IPoolable> _pool;
        private Transform _parent;
        private GameObject _prefab;

        public Pool(IPoolable poolable, Transform parent, int count, PoolTypeSO poolType)
        {
            _pool = new Stack<IPoolable>();
            _parent = parent;
            _prefab = poolable.GameObject;
            for (int i = 0; i < count; i++)
            {
                GameObject gameObject = GameObject.Instantiate(_prefab, _parent);
                gameObject.SetActive(false);
                IPoolable item = gameObject.GetComponent<IPoolable>();
                item.SetUpPool(this);
                item.PoolType = poolType;
                _pool.Push(item);
            }
        }

        public IPoolable Pop()
        {
            IPoolable item;
            if(_pool.Count == 0)
            {
                GameObject gameObject = GameObject.Instantiate(_prefab, _parent);
                item = gameObject.GetComponent<IPoolable>();
                item.SetUpPool(this);
            }
            else
            {
                item = _pool.Pop();
                item.GameObject.SetActive(true);
            }
            item.ResetItem();
            return item;
        }

        public void Push(IPoolable item)
        {
            item.GameObject.SetActive(false);
            _pool.Push(item);
        }
    }
}
