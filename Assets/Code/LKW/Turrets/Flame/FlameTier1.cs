using System;
using UnityEngine;

namespace Code.LKW.Turrets.Flame
{
    public class FlameTier1 : TurretBase
    {
        [SerializeField] private DamageTrigger damageTrigger;
        [SerializeField] private ParticleSystem flameParticle;

        private bool _isFire = false;

        protected override void Start()
        {
            base.Start();
            
            damageTrigger.InitTrigger(damageCompo.CalculateDamage(EntityStatCompo.GetStat(turretDamageStat),
            attackData,
            attackData.damageMultiplier),
            attackData,
            this,
            targetLayer);
        }

        protected override void TryShoot()
        {
            base.TryShoot();

            if (_targets.Count == 0)
            {
                flameParticle.Stop();
            }
        }

        protected override void Shoot()
        {
            flameParticle.Play();
        }
    }
}