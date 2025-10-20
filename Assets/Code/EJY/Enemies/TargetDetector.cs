using System;
using Code.Enemies;
using Code.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public class TargetDetector : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float detectRange = 3.5f;

        private Enemy _enemy;
        private NavMovement _movement;
        private Collider[] _hits;

        public bool IsTargeting { get; private set; }

        public NotifyValue<Transform> CurrentTarget { get; set; } = new();
        public bool InAttackRange { get; private set; }


        private void HandleTargetingChanged(Transform prev, Transform next)
        {
            _movement.SetDestination(next != null ? CurrentTarget.Value.position : _enemy.TargetTrm.position);
        }

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _movement = entity.GetCompo<NavMovement>();
            _hits = new Collider[1];
            CurrentTarget.OnValueChanged += HandleTargetingChanged;
        }

        private void FixedUpdate()
        {
            DetectAndCheckTarget();
        }
        
        void DetectAndCheckTarget()
        {
            if (!IsTargeting)
            {
                Array.Clear(_hits, 0, _hits.Length);

                int detectCount = Physics.OverlapSphereNonAlloc(
                    _enemy.transform.position,
                    detectRange,
                    _hits,
                    whatIsTarget
                );

                if (detectCount > 0)
                {
                    CurrentTarget.Value = _hits[0]?.transform;
                    IsTargeting = CurrentTarget.Value != null;
                }
            }
            else
            {
                if (CurrentTarget.Value == null)
                {
                    IsTargeting = false;
                    InAttackRange = false;
                    return;
                }

                float distance = Vector3.Distance(
                    _enemy.transform.position,
                    CurrentTarget.Value.position
                );

                InAttackRange = distance <= attackRange;

                if (distance > detectRange * 1.2f)
                {
                    IsTargeting = false;
                    CurrentTarget.Value = null;
                    InAttackRange = false;
                }
            }
        }


        private void OnDestroy()
        {
            CurrentTarget.OnValueChanged -= HandleTargetingChanged;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }
}