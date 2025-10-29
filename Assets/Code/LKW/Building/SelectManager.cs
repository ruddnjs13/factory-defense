using Chipmunk.Player;
using Chipmunk.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.LKW.Building
{
    public class SelectManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private LayerMask selectableLayer;
        [SerializeField] private LayerMask floorLayer;

        private ISelectable _selectable;

        private void OnEnable()
        {
            inputReader.OnLeftClickEvent += HandleLeftButtonClick;
        }

        private void HandleLeftButtonClick(bool evt)
        {
            if (UIPointerDetector.IsPointerInUI)
                return;
            
            if (inputReader.MousePositionRaycast(out RaycastHit hit, selectableLayer | floorLayer))
            {
                if (((1 << hit.collider.gameObject.layer) & selectableLayer ) > 0)
                {
                    if (_selectable != null)
                        _selectable.DeSelect();
                
                    _selectable  = hit.collider.gameObject.GetComponent<ISelectable>();

                    if (_selectable != null)
                        _selectable.Select();
                }
                else if (((1 << hit.collider.gameObject.layer) & floorLayer) > 0)
                {
                    if (_selectable != null && (_selectable as MonoBehaviour) != null)
                    {
                        _selectable.DeSelect();
                        _selectable = null;
                    }
                }
                
            }
        }
    }
}