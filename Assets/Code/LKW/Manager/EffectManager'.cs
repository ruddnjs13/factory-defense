using System;
using Code.Effects;
using Code.Events;
using Core.GameEvent;
using RuddnjsLib.Dependencies;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;

namespace Blade.Managers
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO effectChannel;
        [Inject] private PoolManagerMono _poolManager;

        private void Awake()
        {
            effectChannel.AddListener<PlayPoolEffect>(HandlePlayPoolEffect);
        }

        private void OnDestroy()
        {
            effectChannel.RemoveListener<PlayPoolEffect>(HandlePlayPoolEffect);
        }

        private void HandlePlayPoolEffect(PlayPoolEffect evt)
        {
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(evt.targetItem);
            effect.PlayVFX(evt.position, evt.rotation);
            ReturnAfterDelay(effect, evt.duration);
        }

        private async void ReturnAfterDelay(PoolingEffect effect, float evtDuration)
        {
            await Awaitable.WaitForSecondsAsync(evtDuration);
            _poolManager.Push(effect);
        }
    }
}