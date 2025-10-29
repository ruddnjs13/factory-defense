using DG.Tweening;
using RuddnjsPool;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class PopupText : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshPro popUpText;
        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;
        private TextInfoSO _textInfo;

        public void SetUpPool(Pool pool) => _myPool = pool;

        public void ResetItem()
        {
            transform.localScale = Vector3.one;
            popUpText.alpha = 1;
        }

        public void ShowPopupText(string text, TextInfoSO textInfo, Vector3 position, float showDuration)
        {
            _textInfo = textInfo;
            popUpText.font = textInfo.font;
            
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
            seq.AppendCallback(() =>
            {
                _myPool.Push(this);
            });
        }
    }
}