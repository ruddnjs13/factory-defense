using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.LKW.UI
{
    public class MenuButtonUI : MonoBehaviour
    {
        public Action<PanelDataSO> OnButtonClick;
        
        [SerializeField] private Button menuButton;
        [SerializeField] private Image selectBox;
        
        [field: SerializeField] public PanelDataSO TargetPanel { get; set; }

        private void Awake()
        {
            menuButton.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            menuButton.onClick.RemoveListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            OnButtonClick?.Invoke(TargetPanel);
        }

        public void SetActive(bool isActive)
        {
            float targetScale = isActive ? 1f: 0f;

            selectBox.transform.DOKill();
            selectBox.transform.DOScaleX(targetScale, 0.3f).SetUpdate(true);
        }
    }
}