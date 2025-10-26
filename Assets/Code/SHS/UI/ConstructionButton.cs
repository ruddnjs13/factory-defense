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
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color selectedColor = new Color(0.8f, 0.8f, 0.8f);
        private MachineSO machineSo;
        private Color defaultColor;

        public void Enable(MachineSO machineSoArg)
        {
            this.machineSo = machineSoArg;
            button.interactable = true;
            iconImage.sprite = machineSo.Icon;
            button.onClick.AddListener(OnClickHandler);
        }

        private void Awake()
        {
            if (backgroundImage == null)
                backgroundImage = GetComponent<Image>();
            if (backgroundImage != null)
                defaultColor = backgroundImage.color;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<MachineSelectEvent>(OnMachineSelected);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<MachineSelectEvent>(OnMachineSelected);
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

        private void OnMachineSelected(MachineSelectEvent evt)
        {
            // If this button has not been initialized with a MachineSO yet, ignore
            if (backgroundImage == null) return;

            if (evt.MachineSo == machineSo)
            {
                backgroundImage.color = selectedColor;
            }
            else
            {
                backgroundImage.color = defaultColor;
            }
        }
    }
}