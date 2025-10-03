using Blade.Entities;
using UnityEngine;

namespace Blade.Combat
{
    public class WeaponHolder : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Weapon[] weapons;
        [SerializeField] private Transform holderTrm;
        
        public void Initialize(Entity entity)
        {
            weapons = holderTrm.GetComponentsInChildren<Weapon>();
        }
        
        public void Drop()
        {
            foreach (var weapon in weapons)
            {
                weapon.Drop();
            }
        }

        
    }
}