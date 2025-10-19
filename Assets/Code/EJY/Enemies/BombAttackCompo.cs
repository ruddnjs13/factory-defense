using Code.Combat;
using Code.Entities;
using Code.Events;
using RuddnjsPool;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public class BombAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private PoolingItemSO explosionEffectPool;
        [SerializeField] private float effectPlayDuration = 2.5f;

        private OverlapDamageCaster _caster;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _caster = _enemy.GetComponentInChildren<OverlapDamageCaster>();
            _caster.InitCaster(_enemy);
        }

        public override void AfterInitialize()
        {
            base.AfterInitialize();
            _trigger.OnAttackTrigger += Explosion;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _trigger.OnAttackTrigger += Explosion;
        }

        private void Explosion()
        {
            effectChannel.RaiseEvent(EffectEvents.PlayPoolEffect
                .Initializer(_enemy.transform.position,_enemy.transform.rotation ,explosionEffectPool, effectPlayDuration));

            DamageData damageData = _damageCompo.CalculateDamage(damageStat, attackData);
            
            _caster.CastDamage(damageData, _enemy.transform.position, _enemy.transform.forward,attackData);
        }
    }
}