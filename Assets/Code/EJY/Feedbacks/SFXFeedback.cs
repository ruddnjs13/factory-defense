using Blade.Core;
using Blade.Events;
using Blade.Sounds;
using UnityEngine;

namespace Blade.Feedbacks
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