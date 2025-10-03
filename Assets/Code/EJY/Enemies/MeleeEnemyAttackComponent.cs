using System;
using System.Linq;
using Blade.Combat;
using Blade.Core.StatSystem;
using Blade.Entities;
using UnityEngine;

namespace Blade.Enemies
{
    public class MeleeEnemyAttackComponent : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        private Entity _entity;
        private DamageCompo _damageCompo;
        private EntityStatCompo _statCompo;
        private EntityAnimatorTrigger _trigger;

        [SerializeField] private AttackDataSO attackData;
        [SerializeField] private StatSO meleeDamageStat;
        [SerializeField] private OverlapDamageCaster[] casters;
        private bool _isActive;
        
        private DamageData _currentDamageData;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _damageCompo = entity.GetCompo<DamageCompo>();
            _trigger = entity.GetCompo<EntityAnimatorTrigger>();
            casters = entity.GetComponentsInChildren<OverlapDamageCaster>();
            casters.ToList().ForEach(casters => casters.InitCaster(entity));
        }
        
        public void AfterInitialize()
        {
            _trigger.OnDamageToggleTrigger += SetDamageDataCaster;
        }

        private void OnDestroy()
        {
            _trigger.OnDamageToggleTrigger -= SetDamageDataCaster;
        }

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
                    _statCompo.GetStat(meleeDamageStat), attackData);
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