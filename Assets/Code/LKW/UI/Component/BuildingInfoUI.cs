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
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject statContainer;
        [SerializeField] private Image icon;
        [SerializeField] private RectTransform fill;

        private EntityHealth entityHealth;
        
        public void EnableFor(ISelectable selectable)
        {
            if (selectable is BaseMachine machine)
            {
                BuildingInfoSO buildingInfo = machine.BuildingInfo;

                entityHealth = machine.GetCompo<EntityHealth>();
                
                float currentHealth = entityHealth.CurrentHealth;

                entityHealth.onHealthChangedEvent += HandleHealthChanged;
                
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
            entityHealth.onHealthChangedEvent -= HandleHealthChanged;
            entityHealth = null;

        }

        private void HandleHealthChanged(float current, float max)
        {
            healthText.text = $"{current}/{max}";
            fill.localScale = new Vector2(current / max,1);
        }

       


        public void SetBuildingUI(string buildingName, float maxHealth, float currentHealth
            , string description, Sprite iconSprite)
        {
            buildingInfoPanel.SetActive(true);
            statContainer.SetActive(false);
            nameText.text = buildingName;
            descriptionText.text = description;
            healthText.text = $"{currentHealth}/{maxHealth}";
            icon.sprite = iconSprite;
            
            fill.localScale = new Vector2(currentHealth / maxHealth,1);
        }
        
        public void SetBuildingUI(string buildingName, float maxHealth, float currentHealth
            ,float damage, float range, string description, Sprite iconSprite)
        {
            buildingInfoPanel.SetActive(true);
            statContainer.SetActive(true);
            nameText.text = buildingName;
            damageText.text = damage.ToString();
            rangeText.text = range.ToString();
            healthText.text = $"{currentHealth}/{maxHealth}";
            descriptionText.text = description;
            icon.sprite = iconSprite;
            fill.localScale = new Vector2(currentHealth / maxHealth,1);
        }
    }
}
