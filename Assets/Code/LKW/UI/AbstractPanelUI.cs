using System;
using DG.Tweening;
using UnityEngine;

namespace Code.LKW.UI
{
    public abstract class AbstractPanelUI : MonoBehaviour
    {
        [field: SerializeField] public PanelDataSO PanelData { get; private set; }
        [SerializeField] protected float panelHeight = 720f;

        protected RectTransform _rectTrm;

        private bool _isOpen = false;
        private bool _isMoving = false;

        private void Awake()
        {
            _rectTrm = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, -panelHeight);
        }


        public virtual void OpenPanel(bool isTween)
        {
            if (_isOpen || _isMoving)
                return;

            _isMoving = true;
            _rectTrm.DOKill(false);

            if (isTween)
            {
                _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, -panelHeight);

                _rectTrm.DOAnchorPosY(0, 0.3f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        _isOpen = true;
                        _isMoving = false;
                    });
            }
            else
            {
                _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, 0);
                _isOpen = true;
                _isMoving = false;
            }
        }

        public virtual void ClosePanel(bool isTween)
        {
            if (!_isOpen || _isMoving)
                return;

            _isMoving = true;
            _rectTrm.DOKill(false);

            if (isTween)
            {
                _rectTrm.DOAnchorPosY(-panelHeight, 0.3f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        _isOpen = false;
                        _isMoving = false;
                    });
            }
            else
            {
                _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, -panelHeight);
                _isOpen = false;
                _isMoving = false;
            }
        }
    }
}
