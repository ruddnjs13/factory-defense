using Code.Combat;
using Code.Enemies;
using Code.Entities;
using Code.Events;
using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public class RushEnemyAttackCompo : MeleeEnemyAttackCompo
    {
        [SerializeField] private OverlapDamageCaster rushCaster;
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private AttackDataSO rushAttackData;
        [SerializeField] private PoolingItemSO explosionEffectPool;
        [SerializeField] private float effectPlayDuration;
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            rushCaster.InitCaster(entity);
        }

        public void RushEnd()
        {
            cameraChannel?.RaiseEvent(CameraEvents.ImpulseEvent.Initializer(attackData.impulseForce));
            effectChannel?.RaiseEvent(EffectEvents.PlayPoolEffect
                .Initializer(rushCaster.transform.position,Quaternion.identity , explosionEffectPool, effectPlayDuration));

            DamageData damageData = _damageCompo.CalculateDamage(damageStat, rushAttackData);
            
            rushCaster.CastDamage(damageData, _enemy.transform.position, _enemy.transform.forward,attackData);
        }
    }
}