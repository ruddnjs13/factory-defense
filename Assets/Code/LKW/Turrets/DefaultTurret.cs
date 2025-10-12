using Code.LKW.Combat;
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
            
            bullet.transform.position = firePos.position;
            bullet.InitProjectile(firePos.forward);
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