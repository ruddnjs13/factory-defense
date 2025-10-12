using Code.Combat;
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

        [SerializeField] private RangePatternSO rangePattern;
        [SerializeField] private Transform firePos;
        [SerializeField] private float bulletSpeed = 8f;
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

        private void FireBullet()
        {
            Vector3 direction = firePos.forward;
            if (rangePattern.bulletCount == 1)
            {
                Projectile projectile = poolManager.Pop(bulletPool.poolType) as Projectile;

                
                projectile.SetupProjectile(_enemy, _damageCompo.CalculateDamage(damageStat, attackData)
                    , firePos.position, Quaternion.LookRotation(direction), direction * bulletSpeed);
            }
            else
            {
                float startRotation = -(rangePattern.fireAngle * (rangePattern.bulletCount - 1)) / 2;
                for (int i = 0; i < rangePattern.bulletCount; ++i) 
                {
                    Projectile projectile = poolManager.Pop(bulletPool.poolType) as Projectile;
                    float angle = startRotation + i * rangePattern.fireAngle;
                    Quaternion rotation = Quaternion.Euler(0, angle, 0);
                    Vector3 fireDirection = rotation * direction;
                    Quaternion forwardRotation = Quaternion.LookRotation(fireDirection);
                    projectile.SetupProjectile(_enemy, _damageCompo.CalculateDamage(damageStat, attackData)
                        , firePos.position, forwardRotation,fireDirection * bulletSpeed);
                }
            }
        }
    }
}