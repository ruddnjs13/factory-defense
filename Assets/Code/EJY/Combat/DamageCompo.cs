using System;
using Code.Core.StatSystem;
using Code.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Combat
{
    public class DamageCompo : MonoBehaviour, IEntityComponent
    {
        private EntityStatCompo _statCompo;
        
        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStatCompo>();
        }

        public DamageData CalculateDamage(StatSO majorStat, AttackDataSO attackData, float multiplier = 1)
        {
            DamageData data = new DamageData();
            
            data.damage = _statCompo.GetStat(majorStat).Value * attackData.damageMultiplier 
                          + attackData.damageIncrease * multiplier;

            data.damageType = attackData.damageType;
            
            return data;
        }
    }
}