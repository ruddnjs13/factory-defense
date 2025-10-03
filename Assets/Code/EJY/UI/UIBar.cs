using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blade.UI
{
    public class UIBar : MonoBehaviour
    {
        [SerializeField] private Transform barTrm;
        [SerializeField] private TextMeshProUGUI barText;
        [field: SerializeField] public string BarName { get; private set; }

        public void SetText(string text)
        {
            barText.SetText(text);
        }

        public void SetNormalizedValue(float normalizedValue)
        {
            barTrm.DOKill();
            barTrm.DOScaleX(normalizedValue, 0.05f);
        }
    }
}