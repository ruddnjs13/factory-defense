using System;
using System.Collections.Generic;
using Code.Combat;
using Code.EJY.Enemies;
using Code.Entities;
using UnityEngine;

namespace Code.LKW.Turrets.Flame
{
    public class DamageTrigger : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private Vector3 boxSize = new Vector3(3f, 2f, 3f);
        [SerializeField] private float damageTickDelay = 0.4f;
        [SerializeField] private float boxOffset;
        [SerializeField] private LayerMask targetLayer;

        private float _tickCounter = 0f;

        private DamageData _damageData;
        private AttackDataSO _attackData;
        private Entity _dealer;

        private readonly Collider[] _results = new Collider[32];

        public void InitTrigger(DamageData damageData, AttackDataSO attackData, Entity dealer, LayerMask layer)
        {
            _damageData = damageData;
            _attackData = attackData;
            _dealer = dealer;
            targetLayer = layer;
        }

        private void Update()
        {
            _tickCounter += Time.deltaTime;
            if (_tickCounter < damageTickDelay) return;

            ApplyDamageToOverlappedTargets();
            _tickCounter = 0f;
        }

        private void ApplyDamageToOverlappedTargets()
        {
            int count = Physics.OverlapBoxNonAlloc(
                transform.position + transform.forward * boxOffset, 
                boxSize * 0.5f, 
                _results, 
                transform.rotation, 
                targetLayer);

            for (int i = 0; i < count; i++)
            {
                Collider col = _results[i];
                if (col == null) continue;

                if (col.TryGetComponent(out Enemy enemy))
                {
                    if (enemy == null || enemy.IsDead) continue;

                    if (enemy.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        Vector3 hitPoint = enemy.transform.position;
                        Vector3 hitNormal = (transform.position - hitPoint).normalized;

                        damageable.ApplyDamage(_damageData, hitPoint, hitNormal, _attackData, _dealer);
                    }
                }
            }
        }

#if UNITY_EDITOR
        // Scene 뷰에서 감지 범위 시각화
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * boxOffset, transform.rotation, boxSize);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
#endif
    }
}
