using System;
using Code.Combat;
using Code.Entities;
using Code.Events;
using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;
using UnityEngine.Events;

namespace Code.EJY.Enemies
{
    public class BombAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private PoolingItemSO explosionEffectPool;
        [SerializeField] private float effectPlayDuration = 2.5f;

        private OverlapDamageCaster _caster;

        public UnityEvent BeforeExplodeEvent;
        
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _caster = _enemy.GetComponentInChildren<OverlapDamageCaster>();
            _caster.InitCaster(_enemy);
        }

        public override void AfterInitialize()
        {
            base.AfterInitialize();
            _trigger.BeforeAttackTrigger += BeforeExplode;
            _trigger.OnAttackTrigger += Explosion;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _trigger.BeforeAttackTrigger -= BeforeExplode;
            _trigger.OnAttackTrigger -= Explosion;
        }

        private void BeforeExplode() => BeforeExplodeEvent?.Invoke();
        
        private void Explosion()
        {
            cameraChannel?.RaiseEvent(CameraEvents.ImpulseEvent.Initializer(attackData.impulseForce));
            effectChannel?.RaiseEvent(EffectEvents.PlayPoolEffect
                .Initializer(_enemy.transform.position,_enemy.transform.rotation ,explosionEffectPool, effectPlayDuration));

            DamageData damageData = _damageCompo.CalculateDamage(damageStat, attackData);
            
            _caster.CastDamage(damageData, _enemy.transform.position, _enemy.transform.forward,attackData);
        }
    }
}