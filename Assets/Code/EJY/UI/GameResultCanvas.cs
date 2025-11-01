using System;
using System.Text;
using Chipmunk.GameEvents;
using Chipmunk.Player.Events;
using Code.Events;
using Code.SceneSystem;
using Core.GameEvent;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Code.UI
{
    public class GameResultCanvas : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private CanvasGroup totalCanvasGroup, clearCanvasGroup, failCanvasGroup;
        [SerializeField] private TextMeshProUGUI clearTimeText, totalResourceText;
        [SerializeField] private SceneData sceneData;

        public UnityEvent OnClearEvent;
        public UnityEvent OnFailEvent;

        private int _totalResource = 0;
        
        private void Awake()
        {
            SetCanvasGroup(totalCanvasGroup, false);
            
            uiChannel.AddListener<GameResultEvent>(HandleGameResult);
            EventBus<ResourceEvent>.OnEvent += HandleGetResource;
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<GameResultEvent>(HandleGameResult);
            EventBus<ResourceEvent>.OnEvent -= HandleGetResource;
        }

        private void HandleGetResource(ResourceEvent evt)
        {
            if (evt.Amount <= 0) return;
            
            _totalResource += evt.Amount;
        }

        private void HandleGameResult(GameResultEvent evt)
        {
            StringBuilder sb = new StringBuilder();
            
            float clearTime = Time.time;
            int min = (int)clearTime / 60;
            int sec = (int)clearTime % 60;
            
            sb.Append("클리어 시간 ").Append(min.ToString("D2")).Append(":").Append(sec.ToString("D2"));
            clearTimeText.SetText(sb.ToString());
            
            totalResourceText.SetText($"총 생산량 : {_totalResource}");

            DOVirtual.DelayedCall(0.5f, () =>
            {
                Time.timeScale = 0f;
                SetCanvasGroup(totalCanvasGroup, true);
            
                if (evt.isClear)
                {
                    SetCanvasGroup(failCanvasGroup, false);
                    SetCanvasGroup(clearCanvasGroup, true);
                    Clear();
                    sceneData?.SetCanEnter(true);
                }
                else
                {
                    SetCanvasGroup(clearCanvasGroup, false);
                    SetCanvasGroup(failCanvasGroup, true);
                    Fail();
                }
            });
        }

        private void Clear() => OnClearEvent?.Invoke();
        private void Fail() => OnFailEvent?.Invoke();
        
        private void SetCanvasGroup(CanvasGroup targetGroup, bool isActive)
        {
            float alpha = isActive ? 1f : 0f;
            targetGroup.alpha = alpha;
            targetGroup.interactable = isActive;
            targetGroup.blocksRaycasts = isActive;
        }
    }
}