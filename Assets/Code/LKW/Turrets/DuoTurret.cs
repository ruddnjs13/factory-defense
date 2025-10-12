using Code.LKW.Combat;
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
            
            bullet.transform.position = firePos[_shootIdx].position;
            bullet.InitProjectile(firePos[_shootIdx].forward);

            Recoil();
            _shootIdx = (_shootIdx + 1) % 2;
        }

        private void Recoil()
        {
            shooter[_shootIdx].transform.DOLocalMoveZ(-recoilAmount, 0.08f)
                .SetEase(Ease.OutCirc)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}