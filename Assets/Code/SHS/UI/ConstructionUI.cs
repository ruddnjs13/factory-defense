using System;
using System.Collections.Generic;
using Chipmunk.Player;
using UnityEngine;

namespace Chipmunk.UI
{
    public class ConstructionUI : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        private ConstructionTypeButton[] constructionButtons;
        private ConstructionTypeButton previousButton;

        private void Awake()
        {
            constructionButtons = GetComponentsInChildren<ConstructionTypeButton>();
            inputReader.OnNumberKeyEvent += ONumberKeyHandler;

            DisableAllButtons();
        }


        private void ONumberKeyHandler(int obj)
        {
            if (obj < 1 || obj > constructionButtons.Length) return;
            previousButton?.Disable();
            ConstructionTypeButton constructionButton = constructionButtons[obj - 1];
            constructionButton.Enable();
            previousButton = constructionButton;
        }

        private void DisableAllButtons()
        {
            foreach (ConstructionTypeButton button in constructionButtons)
            {
                button.Disable();
            }

            previousButton = null;
        }
    }
}