using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Feedbacks
{
    public class SFXFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private SoundSO[] feedbackSounds;
        
        public override void CreateFeedback()
        {
            int randomIdx = Random.Range(0, feedbackSounds.Length);
            var sfxEvt = SoundsEvents.PlaySfxEvent.Init(transform.position, feedbackSounds[randomIdx]);
            soundChannel.RaiseEvent(sfxEvt);
        }

        public override void StopFeedback()
        {
        }
    }
}