using Code.Combat;
using Code.Events;
using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public class RangeEnemyAttackComponent : EnemyAttackCompo
    {
        [SerializeField] private PoolingItemSO bulletPool;
        [SerializeField] private PoolingItemSO muzzlePool;
        [SerializeField] private GameEventChannelSO effectChannel;
        [SerializeField] private Transform firePos;
        [SerializeField] private float bulletSpeed = 8f;
        [SerializeField] private float maxFireDelay = 0.8f;
        [SerializeField] private float minFireDelay = 0.4f;
        [SerializeField] private PoolManagerSO poolManager;

        private DamageData _currentDamageData;

        public override void AfterInitialize()
        {
            _trigger.OnFireTrigger += FireBullet;
        }

        private void OnDestroy()
        {
            _trigger.OnFireTrigger -= FireBullet;
        }

        public override void Attack()
        {
            base.Attack();
            attackInterval = Random.Range(minFireDelay, maxFireDelay);
        }

        private void FireBullet()
        {
            effectChannel.RaiseEvent(EffectEvents.PlayPoolEffect.Initializer(firePos.position, firePos.rotation, muzzlePool, 1.5f));
            
            Vector3 direction = (_detector.CurrentTarget.Value.position - firePos.position).normalized;
            Projectile projectile = poolManager.Pop(bulletPool.poolType) as Projectile;

            projectile.SetupProjectile(_enemy, _damageCompo.CalculateDamage(damageStat, attackData)
                , firePos.position, Quaternion.LookRotation(direction), direction * bulletSpeed);
        }
    }
}