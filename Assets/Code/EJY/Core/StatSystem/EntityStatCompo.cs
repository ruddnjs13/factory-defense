using System.Collections.Generic;
using System.Linq;
using Blade.Entities;
using UnityEngine;

namespace Blade.Core.StatSystem
{
    public class EntityStatCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatOverride[] statOverrides;
        private Dictionary<string, StatSO> _stats;
        public Entity Owner { get; private set; }
        
        public void Initialize(Entity entity)
        {
            Owner = entity;
            _stats = statOverrides.ToDictionary(s =>s.Stat.statName, s => s.CreateStat());
        }

        public StatSO GetStat(StatSO stat)
        {
            Debug.Assert(stat != null,"Stat cannot be null");
            return _stats.GetValueOrDefault(stat.statName);
        }

        public bool TryGetStat(StatSO stat, out StatSO targetStat)
        {
            Debug.Assert(stat != null,"Stat cannot be null");
            targetStat = _stats.GetValueOrDefault(stat.statName);
            return targetStat != null;
        }
        
        public void SetBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue = value;
        public float GetBaseValue(StatSO stat)=>GetStat(stat).BaseValue;
        public void IncreaseBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue += value;
        public void AddModifier(StatSO stat, object key, float value)
        => GetStat(stat).AddModifier(key, value);
        public void RemoveModifier(StatSO stat, object key)
        => GetStat(stat).RemoveModifier(key);
        public void ClearAllModifiers()
        {
            foreach (var stat in _stats.Values)
            {
                stat.ClearModifier();
            }            
        }

        public float SubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler, float defaultValue)
        {
            StatSO target = GetStat(stat);
            if(target == null) return defaultValue;
            target.OnValueChanged += handler;
            return target.Value;
        }

        public void UnSubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler)
        {
            StatSO target = GetStat(stat);
            if(target == null) return;
            target.OnValueChanged -= handler;
        }
    }
}