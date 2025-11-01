using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using UnityEngine;

namespace Code.LKW.ETC
{
    public class SoundFeedback : MonoBehaviour
    {
        [SerializeField] private SoundSO clickSound;
        [SerializeField] private GameEventChannelSO soundChannel;

        public void CreateFeedback()
        {
            var evt = SoundsEvents.PlaySfxEvent.Init(transform.position, clickSound);
            
            soundChannel.RaiseEvent(evt);
        }
    }
}