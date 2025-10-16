using System;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.Effects
{
    public class PoolingEffect : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;
        [SerializeField] private GameObject effectObject;
        private IPlayableVFX _playableVFX;
        private Tween _delayTween;
        
        private void Awake()
        {
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
        }

        public void ResetItem()
        {
            
        }

        private void OnValidate()
        {
            if(effectObject == null) return;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            if (_playableVFX == null)
            {
                Debug.LogError($"The effect object {effectObject.name} does not implement IPlayableVFX.");
                effectObject = null;
            }
        }

        public void PlayVFX(Vector3 hitPoint, Quaternion rotation, float duration)
        {
            _playableVFX.PlayVfx(hitPoint, rotation);
            _delayTween = DOVirtual.DelayedCall(duration, () =>
                {
                    _myPool.Push(this);
                    _playableVFX.StopVfx();
                })
                .SetLink(gameObject);
        }
    }
}