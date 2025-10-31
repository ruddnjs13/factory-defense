using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.LKW.UI
{
    public class MenuCanvasUI : MonoBehaviour
    {
        private Dictionary<PanelDataSO, MenuButtonUI> _menuButtons = new Dictionary<PanelDataSO, MenuButtonUI>();
        private Dictionary<PanelDataSO, AbstractPanelUI> _menuPanels;

        private AbstractPanelUI _activePanel;
        private void Awake()
        {
            GetComponentsInChildren<MenuButtonUI>(true).ToList().ForEach(btn =>
            {
                btn.OnButtonClick += HandleMenuBtnClick;
                _menuButtons.Add(btn.TargetPanel, btn);
                btn.SetActive(false);
            });

            _menuPanels = GetComponentsInChildren<AbstractPanelUI>().ToDictionary(ui => ui.PanelData);
        }

        private void Start()
        {
            foreach (AbstractPanelUI panel in _menuPanels.Values)
            {
                panel.ClosePanel(false); //트윈없이 바로 닫아.
            }
        }

        private void OnDestroy()
        {
            foreach (MenuButtonUI btn in _menuButtons.Values)
            {
                btn.OnButtonClick -= HandleMenuBtnClick;
            }
        }

        private void HandleMenuBtnClick(PanelDataSO panelData)
        {
            foreach (MenuButtonUI btn in _menuButtons.Values)
            {
                btn.SetActive(btn.TargetPanel == panelData);
            }
            
            _activePanel?.ClosePanel(panelData);
            _activePanel = _menuPanels.GetValueOrDefault(panelData);
            Debug.Assert(_activePanel != default, $"To Target panel exist : {panelData.PanelName}");
            _activePanel.OpenPanel(true);
        }
    }
}