using Code.Entities;
using UnityEngine;

namespace Code.Combat
{
    public abstract class DamageCaster : MonoBehaviour
    {
        [SerializeField] protected LayerMask whatIsEnemy;

        protected Entity _owner;

        public virtual void InitCaster(Entity owner)
        {
            _owner = owner;
        }
        public abstract bool CastDamage(DamageData damageData,Vector3 position,
            Vector3 direction, AttackDataSO attackData);
        
        public virtual void ApplyDamageAndKnockBack(Transform target, DamageData damageData, Vector3 position,
            Vector3 normal, AttackDataSO attackData)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damageData, position, normal, attackData, _owner);
            }
        }
    }
}