using UnityEngine;

namespace Code.LKW.Building
{
    public interface IBuilding
    {
        public float maxHealth { get; set; }
        
        public bool isCombatBuilding { get; set; }
        
        public int damage { get; set; }
        public int range { get; set; }
        
        public string description { get; set; }

        public void Initialize();
    }
}