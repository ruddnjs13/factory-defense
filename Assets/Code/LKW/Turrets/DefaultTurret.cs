using Code.Combat;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Turrets
{
    public class DefaultTurret : TurretBase
    {
        [SerializeField] private float recoilAmount;
        [SerializeField] private Transform firePos;
        [SerializeField] private Transform shooter;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        
        protected override void Shoot()
        {
            Debug.Log("shoot");
            
            Projectile bullet = poolManager.Pop(bulletItem.poolType) as Projectile;
            
            bullet.SetupProjectile(this,damageCompo.CalculateDamage(entityStatCompo.GetStat(turretDamageStat)
                ,attackData),firePos.position ,Quaternion.LookRotation(firePos.forward),firePos.forward *  turretData.bulletSpeed);
            
            Recoil();
        }

        public void Recoil()
        {
            shooter.transform.DOLocalMoveZ(-recoilAmount, 0.08f)
                .SetEase(Ease.OutCirc)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}