using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using UnityEngine;

namespace Code.Feedbacks
{
    public class SFXFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private SoundSO feedbackSound;
        
        public override void CreateFeedback()
        {
            var sfxEvt = SoundsEvents.PlaySfxEvent.Init(transform.position, feedbackSound);
            soundChannel.RaiseEvent(sfxEvt);
        }

        public override void StopFeedback()
        {
        }
    }
}