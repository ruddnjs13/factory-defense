using Code.Combat;
using Code.LKW.Building;
using Code.SHS.Machines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.LKW.UI.Component
{
    
    public class BuildingInfoUI : MonoBehaviour ,IUIElement<ISelectable>
    {
        [SerializeField] private GameObject buildingInfoPanel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI rangeText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject statContainer;
        [SerializeField] private Image icon;

        
        public void EnableFor(ISelectable selectable)
        {
            if (selectable is BaseMachine machine)
            {
                BuildingInfoSO buildingInfo = machine.BuildingInfo;
                float currentHealth = machine.GetCompo<EntityHealth>().CurrentHealth;
                
                if (machine.BuildingInfo.isCombatBuilding)
                {
                    SetBuildingUI(
                        buildingInfo.buildingName,
                        buildingInfo.maxHealth,
                        currentHealth,
                        buildingInfo.damage,
                        buildingInfo.range,
                        buildingInfo.description,
                        buildingInfo.iconSprite
                        );
                }
                else
                {
                    SetBuildingUI(
                        buildingInfo.buildingName,
                        buildingInfo.maxHealth,
                        currentHealth,
                        buildingInfo.description,
                        buildingInfo.iconSprite
                    );
                }
            }
        }

        public void Disable()
        {
            buildingInfoPanel.SetActive(false);

        }


        public void SetBuildingUI(string buildingName, float maxHealth, float currentHealth
            , string description, Sprite iconSprite)
        {
            buildingInfoPanel.SetActive(true);
            statContainer.SetActive(false);
            nameText.text = buildingName;
            descriptionText.text = description;
            icon.sprite = iconSprite;
        }
        
        public void SetBuildingUI(string buildingName, float maxHealth, float currentHealth
            ,float damage, float range, string description, Sprite iconSprite)
        {
            buildingInfoPanel.SetActive(true);
            statContainer.SetActive(true);
            nameText.text = buildingName;
            damageText.text = damage.ToString();
            rangeText.text = range.ToString();
            descriptionText.text = description;
            icon.sprite = iconSprite;
        }
    }
}