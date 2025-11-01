using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.LKW.UI
{
    public class ButtonScaler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        private Vector3 _originScale;

        private void Start()
        {
            _originScale = transform.localScale;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _originScale * 1.1f;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _originScale;
        }
    }
}