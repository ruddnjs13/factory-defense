using System;
using Chipmunk.GameEvents;
using Chipmunk.Player.Events;
using TMPro;
using UnityEngine;

namespace Chipmunk.Player
{
    public class PlayerResource : MonoSingleton<PlayerResource>
    {
        [field: SerializeField] public int Amount { get; private set; } = 1000;
        [SerializeField] private TMP_Text amountText;

        private void Awake()
        {
            EventBus<ResourceEvent>.OnEvent += ResourceEventHandler;
        }

        private void OnDestroy()
        {
            EventBus<ResourceEvent>.OnEvent -= ResourceEventHandler;
        }

        private void ResourceEventHandler(ResourceEvent evt)
        {
            Amount += evt.Amount;
            amountText.SetText(Amount.ToString());
        }
    }
}