using System;
using System.Collections.Generic;
using Code.SHS.Machines.Construction;
using UnityEngine;
using UnityEngine.UI;

namespace Chipmunk.UI
{
    public class ConstructionTypeButton : MonoBehaviour
    {
        [SerializeField] private MachineTypeSO constructionType;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private Image iconImage;
        [SerializeField] private ConstructionButton buttonPrefab;
        private List<ConstructionButton> buttons = new List<ConstructionButton>();
        private ConstructionButton currentButton;

        [SerializeField] private Image backgroundImage;
        private Color defaultColor;
        [SerializeField] private Color selectedColor;

        private void Awake()
        {
            iconImage.sprite = constructionType.Icon;
            foreach (var machineSo in constructionType.machines)
            {
                ConstructionButton button = Instantiate(buttonPrefab, buttonContainer);
                button.Enable(machineSo);
                buttons.Add(button);
            }

            defaultColor = backgroundImage.color;
        }

        public void Enable()
        {
            buttonContainer.gameObject.SetActive(true);
            backgroundImage.color = selectedColor;

            currentButton = buttons[0];
            currentButton.Select();
        }

        public void Disable()
        {
            buttonContainer.gameObject.SetActive(false);
            backgroundImage.color = defaultColor;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && buttonContainer.gameObject.activeSelf)
            {
                if (currentButton == null)
                {
                    currentButton = buttons[0];
                    currentButton.Select();
                }
                else
                {
                    int currentIndex = buttons.IndexOf(currentButton);
                    currentIndex = (currentIndex + 1) % buttons.Count;
                    currentButton = buttons[currentIndex];
                    currentButton.Select();
                }
            }
        }
    }
}