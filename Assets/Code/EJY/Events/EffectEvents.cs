using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;

namespace Code.Events
{
    public static class EffectEvents
    {
        public static PlayPoolEffect PlayPoolEffect = new PlayPoolEffect();
    }

    public class PlayPoolEffect : GameEvent
    {
        public Vector3 position;
        public Quaternion rotation;
        public PoolingItemSO targetItem;
        public float duration;

        public PlayPoolEffect Initializer(Vector3 position, Quaternion rotation, PoolingItemSO targetItem,
            float duration)
        {
            this.position = position;
            this.rotation = rotation;
            this.targetItem = targetItem;
            this.duration = duration;
            
            return this;
        }
    }
}