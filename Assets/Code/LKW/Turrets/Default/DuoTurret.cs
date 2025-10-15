using Code.Combat;
using DG.Tweening;
using RuddnjsPool;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Code.LKW.Turrets
{
    public class DuoTurret : TurretBase
    {
        [SerializeField] private float recoilAmount;
        [SerializeField] private Transform[] firePos;
        [SerializeField] private Transform[] shooter;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;

        private int _shootIdx = 0;
        
        protected override void Shoot()
        {
            Projectile bullet = poolManager.Pop(bulletItem.poolType) as Projectile;
            
            bullet.SetupProjectile(this,damageCompo.CalculateDamage(entityStatCompo.GetStat(turretDamageStat)
                    , attackData),firePos[_shootIdx].position
                , Quaternion.LookRotation(firePos[_shootIdx].forward)
                , firePos[_shootIdx].forward *  turretData.bulletSpeed);

            Recoil();
            _shootIdx = (_shootIdx + 1) % 2;
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