using Chipmunk.ComponentContainers;
using UnityEngine;

namespace Chipmunk.StatSystem
{
    public class StatOverrideBehavior : StatBehavior
    {
        [SerializeField] private StatOverride[] statOverrides;

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            
            stats.Clear();
            foreach (StatOverride statOverride in statOverrides)
            {
                StatSO stat = statOverride.CreateStat();
                stats.Add(stat.statName, stat);
            }
        }
    }
}