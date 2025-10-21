using UnityEngine;

namespace Code.LKW.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Building/Data", order = 0)]
    public class BuildingDataSO : ScriptableObject
    {
        public float maxHealth;
        public bool isCombatBuilding;
        public int damage;
        public int range;
        public string description;
    }
}