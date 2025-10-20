using System;
using System.Globalization;
using Code.UI;
using DG.Tweening;
using RuddnjsPool;
using TMPro;
using UnityEngine;

namespace Blade.UI
{
    public class PopupText : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshPro popUpText;
        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;

        public void SetUpPool(Pool pool) => _myPool = pool;

        public void ResetItem()
        {
            transform.localScale = Vector3.one;
            popUpText.alpha = 1;
        }

        private void LateUpdate()
        {
            Transform mainCamTrm = Camera.main.transform;
            Vector3 camDirection = (transform.position - mainCamTrm.position).normalized;
            transform.forward = camDirection;
        }

        public void ShowPopupText(string text, TextInfoSO textInfo, Vector3 position, float showDuration)
        {
            popUpText.SetText(text);
            popUpText.color = textInfo.textColor;
            popUpText.fontSize = textInfo.fontSize;
            
            transform.position = position;

            float scaleTime = 0.2f;
            float fadeTime = 1.2f;
            
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(2.5f, scaleTime));
            seq.Append(transform.DOScale(1.5f, scaleTime));
            seq.AppendInterval(showDuration);
            seq.Append(transform.DOScale(0.3f,fadeTime));
            seq.Join(popUpText.DOFade(0, fadeTime));
            seq.Join(transform.DOLocalMoveY(position.y + 2f, fadeTime));
            seq.AppendCallback(() =>
            {
                _myPool.Push(this);
            });
        }
    }
}