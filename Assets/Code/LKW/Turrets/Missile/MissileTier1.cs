using Code.Combat;
using Code.Events;
using Code.LKW.Turrets.Combat;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Turrets.Missile
{
    public class MissileTier1 : TurretBase
    {
        [SerializeField] private float recoilAmount;
        [SerializeField] private Transform firePos;
        [SerializeField] private Transform shooter;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        
        
        protected override void Shoot()
        {
            MissileBullet bullet = poolManager.Pop(bulletItem.poolType) as MissileBullet;
            
            bullet.SetTarget(_target.transform, firePos.forward);
            bullet.SetupProjectile(this,damageCompo.CalculateDamage(EntityStatCompo.GetStat(turretDamageStat)
                ,attackData),firePos.position ,Quaternion.LookRotation(firePos.forward),firePos.forward *  turretData.bulletSpeed);
            
            var evt = EffectEvents.PlayPoolEffect.Initializer(firePos.position,
                Quaternion.Euler(firePos.forward), muzzleParticleItem , 0.4f );
            
            effectChannel.RaiseEvent(evt);
            Recoil();
        }

        public void Recoil()
        {
            shooter.transform.DOLocalMoveZ(-recoilAmount, 0.08f)
                .SetEase(Ease.OutCirc)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(gameObject);
        }
    }
}