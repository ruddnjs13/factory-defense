using System;
using Chipmunk.GameEvents;
using Chipmunk.Player;
using Code.LKW.GameEvents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.LKW.Building
{
    public class SelectManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private LayerMask selectableLayer;
        
        [field:SerializeField] public ISelectable Selectable { get; set; }

        private void OnEnable()
        {
            inputReader.OnLeftClickEvent += HandleLeftButtonClick;
        }

        private void HandleLeftButtonClick(bool evt)
        {
            if (inputReader.MousePositionRaycast(out RaycastHit hit, selectableLayer))
            {
                Selectable = hit.collider.gameObject.GetComponent<ISelectable>();

                if (Selectable != null)
                {
                    EventBus<BuildingSelectedEvent>.Raise(new BuildingSelectedEvent(Selectable));
                }
            }
        }
    }
}