using System;
using Code.Events;
using Code.LKW.UI;
using Core.GameEvent;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.EJY.UI
{
    public class StagePopup : AbstractPanelUI
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            SetCanvasGroup(false);
            moveAmount = Screen.width;
            uiChannel.AddListener<SelectStageEvent>(HandleSelectStage);
        }

        protected override void Start()
        {
            RectTrm.anchoredPosition = new Vector2(moveAmount, RectTrm.anchoredPosition.y);
        }

        private void OnDestroy()
        {
            uiChannel.AddListener<SelectStageEvent>(HandleSelectStage);
            
        }

        private void HandleSelectStage(SelectStageEvent evt)
        {
            stageText.text = evt.data.displayName;
            OpenPanel(true);
        }

        private void SetCanvasGroup(bool isOpen)
        {
            float alpha = isOpen ? 1f : 0f;
            canvasGroup.interactable = isOpen;
            canvasGroup.blocksRaycasts = isOpen;
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