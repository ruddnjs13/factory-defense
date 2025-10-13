using System;
using System.Collections.Generic;
using System.Linq;
using Chipmunk.ComponentContainers;
using UnityEngine;

namespace Chipmunk.StatSystem
{
    public class StatBehavior : MonoBehaviour, IContainerComponent
    {
        protected Dictionary<string, StatSO> stats = new();
        public ComponentContainer ComponentContainer { get; set; }

        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
        }

        public void AddStat(StatSO stat)
        {
            stats.Add(stat.statName, stat);
        }

        public StatSO GetStat(StatSO targetStat)
        {
            if (targetStat == null)
            {
                Debug.LogWarning("Stats::GetStat : target stat is null");
                return null;
            }

            return stats.GetValueOrDefault(targetStat?.statName);
        }

        public bool TryGetStat(StatSO targetStat, out StatSO outStat)
        {
            Debug.Assert(targetStat != null, "Stats::GetStat : target stat is null");

            outStat = stats.GetValueOrDefault(targetStat.statName);
            return outStat;
        }

        public void SetBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue = value;
        public float GetBaseValue(StatSO stat) => GetStat(stat).BaseValue;
        public void IncreaseBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue += value;
        public void AddModifier(StatSO stat, object key, float value) => GetStat(stat).AddValueModifier(key, value);
        public void RemoveModifier(StatSO stat, object key) => GetStat(stat).RemoveModifier(key);

        public void CleanAllModifier()
        {
            foreach (StatSO stat in stats.Values)
            {
                stat.ClearModifier();
            }
        }


        #region Save logic

        [Serializable]
        public struct StatSaveData
        {
            public string statName;
            public float baseValue;
        }

        public List<StatSaveData> GetSaveData()
            => stats.Values.Aggregate(new List<StatSaveData>(), (saveList, stat) =>
            {
                saveList.Add(new StatSaveData { statName = stat.statName, baseValue = stat.BaseValue });
                return saveList;
            });


        public void RestoreData(List<StatSaveData> loadedDataList)
        {
            foreach (StatSaveData loadData in loadedDataList)
            {
                StatSO targetStat = stats.GetValueOrDefault(loadData.statName);
                if (targetStat != default)
                {
                    targetStat.BaseValue = loadData.baseValue;
                }
            }
        }

        #endregion

        public List<StatSO> GetAllStats() => stats.Values.ToList();
    }
}