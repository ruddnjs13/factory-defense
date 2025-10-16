using System;

namespace Code.Combat
{
    [Flags]
    public enum DamageType
    {
        None = 0, MELEE = 1, RANGE = 2, BOMB = 4, 
    }
    
    [Serializable]
    public struct DamageData
    {
        public float damage;
        public DamageType damageType;
    }
}