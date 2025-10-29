using DG.Tweening;
using UnityEngine;

namespace Code.LKW.UI
{
    public abstract class AbstractPanelUI : MonoBehaviour
    {
        [field:SerializeField] public PanelDataSO PanelData { get; private set; }

        [SerializeField] protected float panelHeight = 720f;

        protected RectTransform _rectTrm;

        private void Awake()
        {
            _rectTrm = GetComponent<RectTransform>();
            
        }

        public virtual void OpenPanel(bool isTween)
        {
            if (isTween)
            {
                _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, 0);
                _rectTrm.DOKill();
                _rectTrm.DOAnchorPosY(panelHeight, 0.3f).SetUpdate(true);
            }
            else
            {
                _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, panelHeight);
            }
            
        }

        public virtual void ClosePanel(bool isTween)
        {
            if (isTween)
            {
                _rectTrm.DOKill();
                _rectTrm.DOAnchorPosY(0, 0.3f).SetUpdate(true);
            }
            else
            {
                _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, panelHeight);
            }
        }
    }
}