using Code.Combat;
using Code.Core.StatSystem;
using Code.Entities;
using Code.Patterns.PatternDatas;
using RuddnjsLib.Dependencies;
using RuddnjsPool;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.EJY.Enemies
{
    public class RangeEnemyAttackComponent : EnemyAttackCompo
    {
        [SerializeField] private PoolingItemSO bulletPool;

        [SerializeField] private Transform firePos;
        [SerializeField] private PatternSO pattern;
        [SerializeField] private float bulletSpeed = 8f;

        private DamageData _currentDamageData;

        [Inject] private PoolManagerMono _poolManager;
        

        public override void AfterInitialize()
        {
            _trigger.OnFireTrigger += FireBullet;
        }

        private void OnDestroy()
        {
            _trigger.OnFireTrigger -= FireBullet;
        }

        private void FireBullet()
        {
            if (pattern.bulletCount == 1)
            {
                Projectile projectile = _poolManager.Pop<Projectile>(bulletPool);

                projectile.SetupProjectile(_enemy, _damageCompo.CalculateDamage(damageStat, attackData)
                    , firePos.position, projectile.transform.rotation, _enemy.transform.forward * bulletSpeed);
            }
            else
            {
                float startRotation = -(pattern.fireAngle * pattern.bulletCount - 1) / 2;
                
                for (int i = 0; i < pattern.bulletCount; ++i)
                {
                    Projectile projectile = _poolManager.Pop<Projectile>(bulletPool);
                    float angle = startRotation + i * pattern.fireAngle;
                    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                    projectile.SetupProjectile(_enemy, _damageCompo.CalculateDamage(damageStat, attackData)
                        , firePos.position, rotation,  rotation * _enemy.transform.forward * bulletSpeed);
                }
            }
        }
    }
}