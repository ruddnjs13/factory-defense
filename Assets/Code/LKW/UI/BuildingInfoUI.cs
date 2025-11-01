using System;
using Chipmunk.Player;
using Code.Combat;
using Code.LKW.Building;
using Code.LKW.Turrets;
using Code.Managers;
using Code.SHS.Machines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.LKW.UI.Component
{
    
    public class BuildingInfoUI : MonoBehaviour ,IUIElement<ISelectable>
    {
        [Header("Building Info")]
        [SerializeField] private GameObject buildingInfoPanel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI rangeText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject statContainer;
        [SerializeField] private Image icon;
        [SerializeField] private RectTransform fill;

        [Space]
        [Header("Upgrade Info")]
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private TextMeshProUGUI upgradeCostText;

        private TurretBase _turret;
        
        
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
                    if (machine is TurretBase turret)
                    {
                        if (turret.UpgradeIndex <= 1)
                        {
                            upgradePanel.SetActive(true);
                            upgradeCostText.SetText(turret.UpgradeCost.ToString());
                            _turret = turret;
                            
                        }
                    }
                }
                else
                {
                    upgradePanel.SetActive(false);
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
            upgradePanel.SetActive(false);
            _turret = null;
            if (entityHealth != null)
            {
                entityHealth.onHealthChangedEvent -= HandleHealthChanged;
                entityHealth = null;
            }
        }

        private void HandleHealthChanged(float current, float max)
        {
            if (current <= 0)
            {
                Disable();
                return;
            }
            
            healthText.text = $"{current}/{max}";
            fill.localScale = new Vector2(current / max,1);
        }

        public void UpgradeTurret()
        {
            if (PlayerResource.Instance.HasEnoughResource(_turret.UpgradeCost) == false)
            {
                Debug.Log("Not has enough resources to upgrade turret.");
                return;
            }
            
            Debug.Log(_turret.transform.position);
            UpgradeManager.Instance.Upgrade( _turret.Type, _turret.UpgradeIndex+1,_turret.transform.position);
            Destroy(_turret.gameObject);
            
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
