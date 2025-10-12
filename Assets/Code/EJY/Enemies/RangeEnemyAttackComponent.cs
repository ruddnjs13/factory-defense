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

        [SerializeField] private RangePatternSO rangePattern;
        [SerializeField] private Transform firePos;
        [SerializeField] private Projectile projectile; // 임시
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
            Vector3 direction = firePos.forward;
            if (rangePattern.bulletCount == 1)
            {
                //Projectile projectile = _poolManager.Pop<Projectile>(bulletPool);
                Projectile projectile = GameObject.Instantiate(this.projectile, firePos.position, firePos.rotation);

                
                projectile.SetupProjectile(_enemy, _damageCompo.CalculateDamage(damageStat, attackData)
                    , firePos.position, Quaternion.LookRotation(direction), direction * bulletSpeed);
            }
            else
            {
                float startRotation = -(rangePattern.fireAngle * (rangePattern.bulletCount - 1)) / 2;
                for (int i = 0; i < rangePattern.bulletCount; ++i) 
                {
                    //Projectile projectile = _poolManager.Pop<Projectile>(bulletPool);
                    Projectile projectile = GameObject.Instantiate(this.projectile, firePos.position, firePos.rotation);
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