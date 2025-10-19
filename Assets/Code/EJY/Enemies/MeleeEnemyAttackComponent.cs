using System.Linq;
using Code.Combat;
using Code.EJY.Enemies;
using Code.Entities;
using UnityEngine;

namespace Code.Enemies
{
    public class MeleeEnemyAttackComponent : EnemyAttackCompo
    {
        [SerializeField] private OverlapDamageCaster[] casters;
        private bool _isActive;

        private EntityVFX _entityVFX;
        private DamageData _currentDamageData;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _entityVFX = entity.GetCompo<EntityVFX>();
            casters = entity.GetComponentsInChildren<OverlapDamageCaster>();
            casters.ToList().ForEach(casters => casters.InitCaster(entity));
        }

        public override void AfterInitialize()
        {
            base.AfterInitialize();
            _trigger.OnAttackVFXTrigger += HandleAttackVFX;
            _trigger.OnDamageToggleTrigger += SetDamageDataCaster;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _trigger.OnAttackVFXTrigger -= HandleAttackVFX;
            _trigger.OnDamageToggleTrigger -= SetDamageDataCaster;
        }

        private void HandleAttackVFX() => _entityVFX.PlayVfx("Slash" ,Vector3.zero, Quaternion.identity);

        public void SetDamageDataCaster(bool isActive)
        {
            _isActive = isActive;
            if (isActive)
            {
                foreach (var caster in casters)
                {
                    caster.StartCasting();
                }

                _currentDamageData = _damageCompo.CalculateDamage(
                    _statCompo.GetStat(damageStat), attackData);
            }
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                foreach (var caster in casters)
                {
                    caster.CastDamage(_currentDamageData, transform.position,
                        transform.forward, attackData);
                }
            }
        }
    }
}