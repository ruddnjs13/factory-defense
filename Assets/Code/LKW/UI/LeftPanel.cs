using DG.Tweening;
using UnityEngine;

namespace Code.LKW.UI
{
    public class LeftPanel : MonoBehaviour
    {
        [SerializeField] protected GameObject blackPanel;

        [SerializeField] protected float moveAmount = 720f;

        protected RectTransform RectTrm;

        protected bool IsOpen = false;
        protected bool IsMoving = false;
        
        private void Awake()
        {
            RectTrm = GetComponent<RectTransform>();
        }
        
        protected virtual void Start()
        {
            RectTrm.anchoredPosition = new Vector2( -moveAmount, RectTrm.anchoredPosition.y);
            blackPanel.SetActive(false);
        }

        public void HandleButtonClick()
        {
            if (IsOpen == false)
            {
                OpenPanel(true);
            }
            else
            {
                ClosePanel(true);
            }
        }

        
        public  void OpenPanel(bool isTween)
        {
            if (IsMoving)
                return;

            IsMoving = true;
            RectTrm.DOKill(false);
            
            blackPanel.SetActive(true);

            if (isTween)
            {
                RectTrm.anchoredPosition = new Vector2(-moveAmount,RectTrm.anchoredPosition.y);

                RectTrm.DOAnchorPosX(0, 0.3f)
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

        public  void ClosePanel(bool isTween)
        {
            if (IsMoving)
                return;

            IsMoving = true;
            RectTrm.DOKill(false);
            

            if (isTween)
            {
                RectTrm.DOAnchorPosX(-moveAmount, 0.3f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        IsOpen = false;
                        IsMoving = false;
                        blackPanel.SetActive(false);
                    });
            }
            else
            {
                RectTrm.anchoredPosition = new Vector2(-moveAmount,RectTrm.anchoredPosition.y);
                IsOpen = false;
                IsMoving = false;
            }
            
            
        }
    }
}