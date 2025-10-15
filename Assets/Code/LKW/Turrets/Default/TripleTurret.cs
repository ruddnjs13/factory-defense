using Code.Combat;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Turrets
{
    public class TripleTurret : TurretBase
    {
        [SerializeField] private float recoilAmount;
        [SerializeField] private Transform[] firePos;
        [SerializeField] private Transform[] shooter;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        
        private int _shootIdx = 0;
        
        protected override async void Shoot()
        {
            _shootIdx = 0;
            
            ShootProjectile();
            _shootIdx++;

            await Awaitable.WaitForSecondsAsync(0.1f);
            for (int i = 0; i < 2; i++)
            {
                ShootProjectile();
                await Awaitable.WaitForSecondsAsync(0.1f);
                _shootIdx++;
            }
        }

        private void ShootProjectile()
        {
            Projectile bullet = poolManager.Pop(bulletItem.poolType) as Projectile;
            
            bullet.SetupProjectile(this,damageCompo.CalculateDamage(entityStatCompo.GetStat(turretDamageStat)
                    , attackData),firePos[_shootIdx].position
                , Quaternion.LookRotation(firePos[_shootIdx].forward)
                , firePos[_shootIdx].forward *  turretData.bulletSpeed);

            Recoil();
        }

        private void Recoil()
        {
            shooter[_shootIdx].transform.DOLocalMoveZ(-recoilAmount, 0.08f)
                .SetEase(Ease.OutCirc)
                .SetLoops(2, LoopType.Yoyo);
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var s in shooter)
            {
                s.DOKill();
            }
        }
    }
}