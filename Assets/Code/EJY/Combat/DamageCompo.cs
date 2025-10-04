using System;
using Code.Core.StatSystem;
using Code.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Combat
{
    public class DamageCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO criticalStat, criticalDamageStat;

        private EntityStatCompo _statCompo;
        
        private float _critical, _criticalDamage;
        
        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStatCompo>();
        }

        public void AfterInitialize()
        {
            if (criticalStat == null)
                _critical = 0;
            else
            {
                _critical = _statCompo.SubscribeStat(criticalStat ,HandleCriticalChange, 1f);
            }
            
            if (criticalDamageStat == null)
                _criticalDamage = 1;
            else
            {
                _criticalDamage = _statCompo.SubscribeStat(criticalDamageStat ,HandleCriticalDamageChange, 1f);
            }
        }

        private void OnDestroy()
        {
            if (criticalStat != null)
                _statCompo.UnSubscribeStat(criticalStat, HandleCriticalChange);
            
            if (criticalDamageStat != null)
                _statCompo.UnSubscribeStat(criticalDamageStat, HandleCriticalDamageChange);
        }

        private void HandleCriticalChange(StatSO stat, float currentvalue, float previousvalue)
        => _critical = currentvalue;
        private void HandleCriticalDamageChange(StatSO stat, float currentvalue, float previousvalue)
        => _criticalDamage = currentvalue;

        public DamageData CalculateDamage(StatSO majorStat, AttackDataSO attackData, float multiplier = 1)
        {
            DamageData data = new DamageData();
            
            data.damage = _statCompo.GetStat(majorStat).Value * attackData.damageMultiplier 
                          + attackData.damageIncrease * multiplier;
            if (Random.value < _critical)
            {
                data.damage *= _criticalDamage;
                data.isCritical = true;
            }
            else
            {
                data.isCritical = false;
            }

            data.damageType = attackData.damageType;
            
            return data;
        }
    }
}