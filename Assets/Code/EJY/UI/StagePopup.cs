using System;
using System.Collections.Generic;
using System.Linq;
using Code.Events;
using Code.LKW.UI;
using Code.SceneSystem;
using Core.GameEvent;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.EJY.UI
{
    public class StagePopup : AbstractPanelUI
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private GameEventChannelSO sceneChannel;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button enterButton; 

        private List<EnemyIcon> _enemyIcons;
        private string _currentStage;
        
        protected override void Awake()
        {
            base.Awake();
            _enemyIcons = GetComponentsInChildren<EnemyIcon>().ToList();
            SetCanvasGroup(false);
            moveAmount = Screen.width;
            uiChannel.AddListener<SelectStageEvent>(HandleSelectStage);
            enterButton.onClick.AddListener(HandleClickButton);
        }

        protected override void Start()
        {
            RectTrm.anchoredPosition = new Vector2(moveAmount, RectTrm.anchoredPosition.y);
        }

        private void OnDestroy()
        {
            uiChannel.AddListener<SelectStageEvent>(HandleSelectStage);
            enterButton.onClick.RemoveListener(HandleClickButton);
            
        }

        private void HandleClickButton()
        {
            sceneChannel.RaiseEvent(SceneEvents.FadeEvent.Initializer(true, _currentStage));
        }

        private void HandleSelectStage(SelectStageEvent evt)
        {
            stageText.text = evt.data.displayName;
            _currentStage = evt.data.sceneName;
            SetIcons(evt.data);
            OpenPanel(true);
        }

        private void SetCanvasGroup(bool isOpen)
        {
            canvasGroup.interactable = isOpen;
            canvasGroup.blocksRaycasts = isOpen;
        }

        private void SetIcons(SceneData data)
        {
            data.SetEnemyIcons(_enemyIcons);            
        }
        
        public override void OpenPanel(bool isTween)
        {
            if (IsOpen || IsMoving)
                return;

            IsMoving = true;
            RectTrm.DOKill(false);

            if (isTween)
            {
                RectTrm.anchoredPosition = new Vector2(moveAmount, RectTrm.anchoredPosition.y);

                RectTrm.DOAnchorPosX(0, 0.3f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        IsOpen = true;
                        SetCanvasGroup(IsOpen);
                        IsMoving = false;
                    });
            }
            else
            {
                RectTrm.anchoredPosition = new Vector2(RectTrm.anchoredPosition.x, 0);
                IsOpen = true;
                IsMoving = false;
            }
        }

        public override void ClosePanel(bool isTween)
        {
            if (!IsOpen || IsMoving)
                return;

            IsMoving = true;
            RectTrm.DOKill(false);

            if (isTween)
            {
                RectTrm.DOAnchorPosX(moveAmount, 0.3f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        IsOpen = false;
                        SetCanvasGroup(IsOpen);
                        IsMoving = false;
                    });
            }
            else
            {
                RectTrm.anchoredPosition = new Vector2(moveAmount,RectTrm.anchoredPosition.y);
                IsOpen = false;
                IsMoving = false;
            }
        }
    }
}