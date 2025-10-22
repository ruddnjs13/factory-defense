using UnityEngine;

namespace Code.LKW.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Building/Data", order = 0)]
    public class BuildingInfoSO : ScriptableObject
    {
        public string buildingName;
        public float maxHealth;
        public bool isCombatBuilding;
        public int damage;
        public int range;
        public Sprite iconSprite;
        [TextArea]
        public string description;
        
    }
}