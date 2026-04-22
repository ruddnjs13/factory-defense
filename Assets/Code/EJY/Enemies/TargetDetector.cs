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
        [SerializeField, Min(1)] private int maxTargetCount = 16;

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
            _hits = new Collider[Mathf.Max(1, maxTargetCount)];
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
                Transform closestTarget = FindClosestTarget();

                if (closestTarget != null)
                {
                    CurrentTarget.Value = closestTarget;
                    IsTargeting = true;
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

        private Transform FindClosestTarget()
        {
            Array.Clear(_hits, 0, _hits.Length);

            int detectCount = Physics.OverlapSphereNonAlloc(
                _enemy.transform.position,
                detectRange,
                _hits,
                whatIsTarget
            );

            Transform closestTarget = null;
            float closestSqrDistance = float.MaxValue;
            Vector3 origin = _enemy.transform.position;

            for (int i = 0; i < detectCount && i < _hits.Length; i++)
            {
                Collider hit = _hits[i];
                if (hit == null) continue;

                Transform target = hit.transform;
                float sqrDistance = (target.position - origin).sqrMagnitude;

                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }


        private void OnDestroy()
        {
            CurrentTarget.OnValueChanged -= HandleTargetingChanged;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }
}