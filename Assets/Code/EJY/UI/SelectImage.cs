using System;
using Code.Events;
using Code.SceneSystem;
using Core.GameEvent;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.EJY.UI
{
    public class SelectImage : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image boxImage;
        [SerializeField] private Image circleImage;
        [SerializeField] private float tweenDuration = 0.5f;
        [SerializeField] private float moveDuration = 0.8f;
        [SerializeField] private float boxPixelPerMultiplier = 2f;
        [SerializeField] private float circleSizeMultiplier = 2f;

        private Vector3 _originScale;
        private Vector3 _originCircleScale;
        private SceneData _currentSceneData;
        
        private RectTransform Rect => transform as RectTransform;
        
        private void Awake()
        {
            _originScale = transform.localScale;
            _originCircleScale = circleImage.transform.localScale;
            canvasGroup.alpha = 0;
            
            uiChannel.AddListener<SelectStageEvent>(HandleSelectImage);
        }

        private void OnDestroy()
        {
            boxImage.DOKill();
            circleImage.DOKill();
            uiChannel.RemoveListener<SelectStageEvent>(HandleSelectImage);
        }

        private void HandleSelectImage(SelectStageEvent evt)
        {
            if (_currentSceneData == evt.data) return;
            _currentSceneData = evt.data;
            EnableImage(evt.position);
        }

        private void EnableImage(Vector3 position)
        {
            bool isFirst = Mathf.Approximately(canvasGroup.alpha, 0f);
            
            if (isFirst)
            {
                MoveImage(position, false);
                Sequence seq = DOTween.Sequence();
                seq.Append(DOTween.To(() => boxImage.pixelsPerUnitMultiplier
                    , x => boxImage.pixelsPerUnitMultiplier = x, boxPixelPerMultiplier, tweenDuration))
                    .Join(circleImage.transform.DOScale(_originCircleScale * circleSizeMultiplier, tweenDuration))
                    .SetLoops(-1, LoopType.Yoyo);
                canvasGroup.alpha = 1;
            }
            else
            {
                MoveImage(position, true);
            }
        }

        private void MoveImage(Vector3 position, bool isTween)
        {
            if (isTween)
            {
                Rect.DOScale(_originScale * 0.3f, 0.1f);
                Rect.DOAnchorPos(position, moveDuration).OnComplete(() => Rect.DOScale(_originScale, 0.1f));
            }
            else
            {
                Rect.anchoredPosition = position;
            }
        }
    }
}