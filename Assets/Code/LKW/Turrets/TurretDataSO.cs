using Code.Managers;
using UnityEngine;

namespace Code.LKW.Turrets
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "Turret/TurretData", order = 0)]
    public class TurretDataSO : ScriptableObject
    {
        [Header("Stats")]
        public float range;
        public float reloadTimer;
        public float rotationSpeed;
        public float bulletSpeed;
        public float shootAllowanceAngle;

        [Space] [Header("Upgrade")] 
        public TurretType type;
        public int upgradeIndex;
        public int upgradeCost;
        //public 
    }
}