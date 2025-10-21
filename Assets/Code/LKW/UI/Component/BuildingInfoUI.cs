using Code.LKW.Building;
using Code.SHS.Machines;
using TMPro;
using UnityEngine;

namespace Code.LKW.UI.Component
{
    public class BuildingInfoUI : IUIElement<ISelectable>
    {
        public void EnableFor(ISelectable selectable)
        {
            if (selectable is BaseMachine machine)
            {
                if (machine.BuildingInfo.isCombatBuilding)
                {
                    
                }
            }
        }

        public void Disable()
        {
        }


        public void SetBuildingUI(string buildingName, float maxHealth, string description)
        {
            
        }
    }
}