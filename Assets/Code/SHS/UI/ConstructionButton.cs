using System;
using Chipmunk.GameEvents;
using Code.SHS.Machines;
using Code.SHS.Machines.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Chipmunk.UI
{
    public class ConstructionButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        private MachineSO machineSo;

        public void Enable(MachineSO machineSo)
        {
            this.machineSo = machineSo;
            button.interactable = true;
            iconImage.sprite = machineSo.Icon;
            button.onClick.AddListener(OnClickHandler);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClickHandler);
        }

        private void OnClickHandler()
        {
            Select();
        }

        public void Select()
        {
            EventBus.Raise(new MachineSelectEvent(machineSo));
        }
    }
}