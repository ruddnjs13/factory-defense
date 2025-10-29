using System;
using System.Collections.Generic;
using System.Linq;
using Code.Events;
using Code.UI;
using Core.GameEvent;
using RuddnjsLib.Dependencies;
using RuddnjsPool;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;

namespace Code.EJY.Manager
{
    public class TextManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO textChannel;
        [SerializeField] private PoolingItemSO popupItem;

        [Inject] private PoolManagerMono _poolManager;

        [SerializeField] private TextInfoSO[] textInfos;
        private Dictionary<int, TextInfoSO> _textInfoDict;

        private void Awake()
        {
            textChannel.AddListener<PopupTextEvent>(HandlePopupTextEvent);
            _textInfoDict = textInfos.ToDictionary(x => x.nameHash);
        }

        private void OnDestroy()
        {
            textChannel.RemoveListener<PopupTextEvent>(HandlePopupTextEvent);
        }

        private void HandlePopupTextEvent(PopupTextEvent evt)
        {
            PopupText popupText = _poolManager.Pop<PopupText>(popupItem);
            TextInfoSO textInfo = _textInfoDict.GetValueOrDefault(evt.textTypeHash);
            
            popupText.ShowPopupText(evt.text, textInfo, evt.position, evt.showDuration);
        }
    }
}