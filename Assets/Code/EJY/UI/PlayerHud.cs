using System;
using System.Collections.Generic;
using System.Linq;
using Code.Events;
using Core.GameEvent;
using UnityEngine;

namespace Code.UI
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        
        private Dictionary<string, UIBar> _bars;

        private void Awake()
        {
            _bars = GetComponentsInChildren<UIBar>().ToDictionary(x => x.BarName);
            
            playerChannel.AddListener<PlayerHealthEvent>(HandleHealthEvent);
            playerChannel.AddListener<PlayerExpEvent>(HandleExpEvent);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<PlayerHealthEvent>(HandleHealthEvent);
            playerChannel.RemoveListener<PlayerExpEvent>(HandleExpEvent);
        }

        private void HandleHealthEvent(PlayerHealthEvent evt)
        {
            const string healthBarName = "HealthBar";
            UIBar target = _bars.GetValueOrDefault(healthBarName);
            SetTargetBar(target, evt.health, evt.maxHealth);
        }

        private void HandleExpEvent(PlayerExpEvent evt)
        {
            const string healthBarName = "ExpBar";
            UIBar target = _bars.GetValueOrDefault(healthBarName);
            SetTargetBar(target, evt.currentExp, evt.maxExp  );
        }

        private void SetTargetBar(UIBar target, float value, float maxValue)
        {
            target.SetText($"{value}/{maxValue}");
            target.SetNormalizedValue(value / maxValue);
        }
    }
}