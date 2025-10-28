using System;
using System.Collections.Generic;
using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using RuddnjsLib.Dependencies;
using RuddnjsPool;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;

namespace Blade.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private PoolingItemSO soundPlayer;
        [Inject] private PoolManagerMono _poolManager;

        private Dictionary<int, SoundPlayer> _longSoundDict;
        
        private SoundPlayer _bgmPlayer;

        private void Awake()
        {
            _longSoundDict = new Dictionary<int, SoundPlayer>();
            soundChannel.AddListener<PlaySFXEvent>(HandlePlaySFX);
            soundChannel.AddListener<PlayBGMEvent>(HandlePlayBGM);
            soundChannel.AddListener<StopBGMEvent>(HandleStopSFX);
            soundChannel.AddListener<PlayLongSFXEvent>(HandlePlayLongSFX);
            soundChannel.AddListener<StopLongSFXEvent>(HandleStopLongSFX);
        }

        private void HandleStopLongSFX(StopLongSFXEvent evt)
        {
            if (_longSoundDict.TryGetValue(evt.id, out SoundPlayer player))
            {
                player.StopAndReturnPool();
                _longSoundDict.Remove(evt.id);
            }
        }

        private void HandlePlayLongSFX(PlayLongSFXEvent evt)
        {
            if (_longSoundDict.TryGetValue(evt.id, out SoundPlayer player))
            {
                player.StopAndReturnPool();
                _longSoundDict.Remove(evt.id);
            }
            SoundPlayer sfxPlayer = _poolManager.Pop<SoundPlayer>(soundPlayer);
            sfxPlayer.PlaySound(evt.soundClip);
            
            _longSoundDict.Add(evt.id, sfxPlayer);
        }

        private void OnDestroy()
        {
            soundChannel.RemoveListener<PlaySFXEvent>(HandlePlaySFX);
            soundChannel.RemoveListener<PlayBGMEvent>(HandlePlayBGM);
            soundChannel.RemoveListener<StopBGMEvent>(HandleStopSFX);
        }

        private void HandlePlaySFX(PlaySFXEvent evt)
        {
            SoundPlayer sfxPlayer = _poolManager.Pop<SoundPlayer>(soundPlayer);
            sfxPlayer.transform.position = evt.position;
            sfxPlayer.PlaySound(evt.soundClip);
        }


        private void HandlePlayBGM(PlayBGMEvent evt)
        {
            _bgmPlayer = _poolManager.Pop<SoundPlayer>(soundPlayer);
            _bgmPlayer.PlaySound(evt.soundClip);
        }
        
        private void HandleStopSFX(StopBGMEvent evt)
        {
            _bgmPlayer?.StopAndReturnPool();
        }
    }
}