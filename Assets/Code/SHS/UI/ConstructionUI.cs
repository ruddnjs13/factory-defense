using System;
using System.Collections.Generic;
using Chipmunk.Player;
using UnityEngine;

namespace Chipmunk.UI
{
    public class ConstructionUI : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private List<ConstructionTypeButton> constructionButtons;
        private ConstructionTypeButton previousButton;

        private void Awake()
        {
            inputReader.OnNumberKeyEvent += ONumberKeyHandler;
        }

        private void ONumberKeyHandler(int obj)
        {
            if (obj < 1 || obj > constructionButtons.Count) return;
            previousButton?.Disable();
            ConstructionTypeButton constructionButton = constructionButtons[obj - 1];
            constructionButton.Enable();
            previousButton = constructionButton;
        }
    }
}