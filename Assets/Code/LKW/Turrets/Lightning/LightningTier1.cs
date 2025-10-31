using Code.Combat;
using Code.Events;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Turrets.Lightning
{
    public class LightningTier1 : TurretBase
    {
        [SerializeField] private float recoilAmount;
        [SerializeField] private Transform firePos;
        [SerializeField] private Transform shooter;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        
        
        protected override void Shoot()
        {
            ChainLightning bullet = poolManager.Pop(bulletItem.poolType) as ChainLightning;
            
            bullet.SetupProjectile(this,damageCompo.CalculateDamage(EntityStatCompo.GetStat(turretDamageStat)
                ,attackData),firePos.position ,Quaternion.LookRotation(firePos.forward),firePos.forward *  turretData.bulletSpeed);
            
            var evt = EffectEvents.PlayPoolEffect.Initializer(firePos.position,
                Quaternion.Euler(firePos.forward), muzzleParticleItem , 0.4f );
            
            effectChannel.RaiseEvent(evt);
            
            var soundEvt = SoundsEvents.PlaySfxEvent.Init(firePos.position, shootSound);
            
            soundChannel.RaiseEvent(soundEvt);
            Recoil();
        }

        public void Recoil()
        {
            shooter.DOKill();

            shooter.transform.DOLocalMoveZ(-recoilAmount, 0.08f)
                .SetEase(Ease.OutCirc)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(shooter.gameObject);
        }
    }
}