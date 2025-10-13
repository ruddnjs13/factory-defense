using System;
using System.Collections.Generic;
using Code.SHS.Machines.Construction;
using UnityEngine;

namespace Chipmunk.UI
{
    public class ConstructionTypeButton : MonoBehaviour
    {
        [SerializeField] private MachineTypeSO constructionType;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private ConstructionButton buttonPrefab;
        private List<ConstructionButton> buttons = new List<ConstructionButton>();
        private ConstructionButton currentButton;
        private void Awake()
        {
            foreach (var machineSo in constructionType.machines)
            {
                ConstructionButton button = Instantiate(buttonPrefab, buttonContainer);
                button.Enable(machineSo);
                buttons.Add(button);
            }
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (currentButton == null)
                {
                    currentButton = buttons[0];
                    currentButton.Select();
                }
                else
                {
                    int currentIndex = buttons.IndexOf(currentButton);
                    // currentButton.Deselect();
                    currentIndex = (currentIndex + 1) % buttons.Count;
                    currentButton = buttons[currentIndex];
                    currentButton.Select();
                }
            }
        }
    }
}