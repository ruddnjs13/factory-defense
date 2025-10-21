using System;
using Chipmunk.GameEvents;
using Code.LKW.GameEvents;
using UnityEngine;

namespace Code.LKW.UI
{
    public class RuntimeUI : MonoBehaviour
    {
        private void OnEnable()
        {
            EventBus<BuildingSelectedEvent>.OnEvent += HandleBuildingSelected;
        }

        private void HandleBuildingSelected(BuildingSelectedEvent evt)
        {
            
        }
    }
}