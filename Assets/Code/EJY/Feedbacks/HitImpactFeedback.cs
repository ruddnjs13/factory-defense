using Code.Combat;
using Code.Effects;
using Code.Entities;
using DG.Tweening;
using RuddnjsLib.Dependencies;
using RuddnjsPool;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;

namespace Code.Feedbacks
{
    public class HitImpactFeedback : Feedback
    {
        [SerializeField] private PoolingItemSO hitImpactItem;
        [SerializeField] private float playDuration = 0.5f;
        [SerializeField] private ActionData actionData;
        [SerializeField] private DamageType allowedDamageType;
        
        [Inject] private PoolManagerMono _poolManager;

        private PoolingEffect _effect;
        
        public override void CreateFeedback()
        {
            if ((actionData.DamageData.damageType & allowedDamageType) == 0) return;
            
            _effect = _poolManager.Pop<PoolingEffect>(hitImpactItem);
            
            Quaternion rotation = Quaternion.LookRotation(actionData.HitNormal * -1);
            _effect.PlayVFX(actionData.HitPoint, rotation);

            DOVirtual.DelayedCall(playDuration, StopFeedback);
        }

        public override void StopFeedback()
        {
            if (_effect == null) return;
            _poolManager.Push(_effect);
        }
    }
}