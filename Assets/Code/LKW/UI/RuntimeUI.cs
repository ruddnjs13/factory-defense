using Chipmunk.GameEvents;
using Code.LKW.GameEvents;
using Code.LKW.UI.Component;
using UnityEngine;

namespace Code.LKW.UI
{
    public class RuntimeUI : MonoBehaviour
    {
        [SerializeField] private BuildingInfoUI buildingInfoUI;
        
        private void OnEnable()
        {
            EventBus<BuildingSelectedEvent>.OnEvent += HandleBuildingSelected;
            EventBus<BuildingDeselectEvent>.OnEvent += HandleBuildingDeSelected;
        }

        private void HandleBuildingDeSelected(BuildingDeselectEvent evt)
        {
            buildingInfoUI.Disable();

        }

        private void HandleBuildingSelected(BuildingSelectedEvent evt)
        {
            buildingInfoUI.EnableFor(evt.Selectable);
        }
    }
}