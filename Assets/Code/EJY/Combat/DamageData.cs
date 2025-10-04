using System;

namespace Code.Combat
{
    [Flags]
    public enum DamageType
    {
        None = 0, MELEE = 1, RANGE = 2, MAGIC = 4
    }
    
    public struct DamageData
    {
        public float damage;
        public bool isCritical;
        public DamageType damageType;

    }
}