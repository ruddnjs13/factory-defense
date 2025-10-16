using Code.Combat;
using Code.Effects;
using Code.Events;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Turrets
{
    public class DefaultTier1 : TurretBase
    {
        [SerializeField] private float recoilAmount;
        [SerializeField] private Transform firePos;
        [SerializeField] private Transform shooter;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        
        
        protected override void Shoot()
        {
            Projectile bullet = poolManager.Pop(bulletItem.poolType) as Projectile;
            
            bullet.SetupProjectile(this,damageCompo.CalculateDamage(entityStatCompo.GetStat(turretDamageStat)
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
                .SetLoops(2, LoopType.Yoyo);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            shooter.DOKill();
        }
    }
}