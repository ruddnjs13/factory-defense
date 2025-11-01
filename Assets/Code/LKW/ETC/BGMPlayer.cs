using System;
using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using UnityEngine;

namespace Code.LKW.ETC
{
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField] private SoundSO bgmSound;
        [SerializeField] private GameEventChannelSO soundChannel;
        
        private void Start()
        {
            var evt = SoundsEvents.PlayBGMEvent.Init(bgmSound);
            soundChannel.RaiseEvent(evt);
        }
    }
}