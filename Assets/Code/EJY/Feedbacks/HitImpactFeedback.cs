using System.Collections.Generic;
using System.Linq;
using Code.Combat;
using Code.Effects;
using Code.Entities;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.Feedbacks
{
    public class HitImpactFeedback : Feedback
    {
        [SerializeField] private float playDuration = 0.5f;
        [SerializeField] private ActionData actionData;
        
        [SerializeField] private PoolManagerSO poolManager;

        private PoolingEffect _effect;
        private Dictionary<DamageType, PoolingItemSO> _hitImpactDict;

        private void Awake()
        {
            _hitImpactDict = new Dictionary<DamageType, PoolingItemSO>();
            GetComponentsInChildren<HitImpact>().ToList()
                .ForEach(x => _hitImpactDict.Add(x.AllowedDamageType, x.PoolingItem));   
        }

        public override void CreateFeedback()
        {
            PoolingItemSO hitPoolItem = _hitImpactDict.GetValueOrDefault(actionData.DamageData.damageType); 
            Debug.Assert(hitPoolItem != null, $"DamageType can not found, damage type is {actionData.DamageData.damageType}");
            _effect = poolManager.Pop(hitPoolItem.poolType) as PoolingEffect;
            
            Quaternion rotation = Quaternion.LookRotation(actionData.HitNormal * -1);
            _effect.PlayVFX(actionData.HitPoint, rotation);

            DOVirtual.DelayedCall(playDuration, StopFeedback);
        }

        public override void StopFeedback()
        {
            if (_effect == null) return;
            poolManager.Push(_effect);
        }
    }
}