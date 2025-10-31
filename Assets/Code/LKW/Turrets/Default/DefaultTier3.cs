using System;
using System.Threading;
using Code.Combat;
using Code.Events;
using DG.Tweening;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Turrets
{
    public class DefaultTier3 : TurretBase
    {
        [SerializeField] private float recoilAmount;
        [SerializeField] private Transform[] firePos;
        [SerializeField] private Transform[] shooter;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        
        private int _shootIdx = 0;
        private CancellationTokenSource _cts;

        private void Awake()
        {
            base.Awake();
            _cts = new CancellationTokenSource();
        }

        protected override async void Shoot()
        {
            _shootIdx = 0;
            ShootProjectile();
            _shootIdx++;

            try
            {
                await Awaitable.WaitForSecondsAsync(0.1f, _cts.Token);
                for (int i = 0; i < 2; i++)
                {
                    ShootProjectile();
                    await Awaitable.WaitForSecondsAsync(0.1f, _cts.Token);
                    _shootIdx = (_shootIdx + 1) % 3;
                }
            }
            catch (OperationCanceledException)
            {
                // Turret이 파괴되거나 비활성화될 때 안전하게 중단됨
            }
        }

        private void ShootProjectile()
        {
            Projectile bullet = poolManager.Pop(bulletItem.poolType) as Projectile;
            
            bullet.SetupProjectile(this, 
                damageCompo.CalculateDamage(EntityStatCompo.GetStat(turretDamageStat), attackData),
                firePos[_shootIdx].position,
                Quaternion.LookRotation(firePos[_shootIdx].forward),
                firePos[_shootIdx].forward * turretData.bulletSpeed);

            var evt = EffectEvents.PlayPoolEffect.Initializer(firePos[_shootIdx].position,
                Quaternion.Euler(firePos[_shootIdx].forward), muzzleParticleItem , 0.4f );
            
            var soundEvt = SoundsEvents.PlaySfxEvent.Init(firePos[_shootIdx].position, shootSound);
            
            soundChannel.RaiseEvent(soundEvt);
            
            effectChannel.RaiseEvent(evt);

            Recoil();
        }

        private void Recoil()
        {
            shooter[_shootIdx].DOLocalMoveZ(-recoilAmount, 0.08f)
                .SetEase(Ease.OutCirc)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        public override void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
