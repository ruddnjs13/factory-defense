using System;
using Code.Combat;
using Code.Core.StatSystem;
using Code.Entities;
using RuddnjsLib.Dependencies;
using RuddnjsPool;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public class RangeEnemyAttackComponent : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private PoolingItemSO bulletPool;
        [SerializeField] private AttackDataSO attackData;
        [SerializeField] private StatSO meleeDamageStat;
        [SerializeField] private Transform[] firePoints;
        
        private Entity _entity;
        private DamageCompo _damageCompo;
        private EntityStatCompo _statCompo;
        private EntityAnimatorTrigger _trigger;
        
        private DamageData _currentDamageData;

        [Inject] private PoolManagerMono _poolManager;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _damageCompo = entity.GetCompo<DamageCompo>();
            _trigger = entity.GetCompo<EntityAnimatorTrigger>();
        }

        public void AfterInitialize()
        {
            _trigger.OnFireTrigger += FireBullet;
        }

        private void OnDestroy()
        {
            _trigger.OnFireTrigger -= FireBullet;
        }

        private void FireBullet()
        {
            Projectile projectile = _poolManager.Pop<Projectile>(bulletPool);
            //projectile.SetupProjectile(_entity, _damageCompo.CalculateDamage(meleeDamageStat, attackData)
            //    , firePoints[0].position, projectile.transform.rotation);
        }
    }
}