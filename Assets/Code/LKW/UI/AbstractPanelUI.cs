using DG.Tweening;
using UnityEngine;

namespace Code.LKW.UI
{
    public abstract class AbstractPanelUI : MonoBehaviour
    {
        [field: SerializeField] public PanelDataSO PanelData { get; private set; }
        [SerializeField] protected float moveAmount = 720f;

        protected RectTransform RectTrm;

        protected bool IsOpen = false;
        protected bool IsMoving = false;

        protected virtual void Awake()
        {
            RectTrm = GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {
            RectTrm.anchoredPosition = new Vector2(RectTrm.anchoredPosition.x, -moveAmount);
        }


        public virtual void OpenPanel(bool isTween)
        {
            if (IsOpen || IsMoving)
                return;

            IsMoving = true;
            RectTrm.DOKill(false);

            if (isTween)
            {
                RectTrm.anchoredPosition = new Vector2(RectTrm.anchoredPosition.x, -moveAmount);

                RectTrm.DOAnchorPosY(0, 0.3f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        IsOpen = true;
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

        public virtual void ClosePanel(bool isTween)
        {
            if (!IsOpen || IsMoving)
                return;

            IsMoving = true;
            RectTrm.DOKill(false);

            if (isTween)
            {
                RectTrm.DOAnchorPosY(-moveAmount, 0.3f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        IsOpen = false;
                        IsMoving = false;
                    });
            }
            else
            {
                RectTrm.anchoredPosition = new Vector2(RectTrm.anchoredPosition.x, -moveAmount);
                IsOpen = false;
                IsMoving = false;
            }
        }
    }
}
