using Chipmunk.Player;
using UnityEngine;

namespace Code.LKW.Building
{
    public class SelectManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private LayerMask selectableLayer;

        private ISelectable _selectable;

        private void OnEnable()
        {
            inputReader.OnLeftClickEvent += HandleLeftButtonClick;
        }

        private void HandleLeftButtonClick(bool evt)
        {
            if (inputReader.MousePositionRaycast(out RaycastHit hit, selectableLayer))
            {
                _selectable  = hit.collider.gameObject.GetComponent<ISelectable>();

                if (_selectable != null)
                {
                    _selectable.Select();
                }
            }
            else
            {
                if (_selectable != null)
                {
                    _selectable.DeSelect();
                    _selectable = null;
                }
            }
        }
    }
}