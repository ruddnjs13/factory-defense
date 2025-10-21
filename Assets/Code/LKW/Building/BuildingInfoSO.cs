using UnityEngine;

namespace Code.LKW.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Building/Data", order = 0)]
    public class BuildingInfoSO : ScriptableObject
    {
        public float maxHealth;
        public bool isCombatBuilding;
        public int damage;
        public int range;
        [TextArea]
        public string description;
    }
}