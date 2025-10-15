using Code.Events;
using Code.Feedbacks;
using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.FeedbackS
{
    public class BuildingDestroyFeedback : Feedback
    {
        [SerializeField] private PoolingItemSO effectItem;
        [SerializeField] private GameEventChannelSO effectChannel;
        
        public override void CreateFeedback()
        {
            var evt = EffectEvents.PlayPoolEffect.Initializer(
                transform.position,
                transform.rotation,
                effectItem,
                0.8f);
            
            effectChannel.RaiseEvent(evt);
        }

        public override void StopFeedback()
        {
            
        }
    }
}