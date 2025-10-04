using System;
using System.Collections.Generic;
using Code.Entities;
using UnityEngine;

namespace Code.Combat
{
    public class OverlapDamageCaster : DamageCaster
    {
        [SerializeField] private float castRadius;
        [SerializeField] private int maxCollideCount;
        
        private Collider[] _colliders;
        private HashSet<Transform> _hitObjects;

        public override void InitCaster(Entity owner)
        {
            base.InitCaster(owner);
            _colliders = new Collider[maxCollideCount];
            _hitObjects = new HashSet<Transform>(maxCollideCount);
        }

        public void StartCasting()
        {
            _hitObjects.Clear();
        }
        
        public override bool CastDamage(DamageData damageData, Vector3 position, Vector3 direction, AttackDataSO attackData)
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, castRadius, _colliders, whatIsEnemy);
            
            for(int i = 0; i < count; i++)
            {
                Transform target = _colliders[i].transform;
                
                if (_hitObjects.Contains(target.root))
                    continue;

                if (_hitObjects.Count >= maxCollideCount)
                    continue;
                
                _hitObjects.Add(target.root);
                
                Vector3 normal = (_owner.transform.position - target.position).normalized;
                ApplyDamageAndKnockBack(target, damageData, transform.position, normal, attackData);
            }

            return count > 0;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, castRadius);
        }
    }
}