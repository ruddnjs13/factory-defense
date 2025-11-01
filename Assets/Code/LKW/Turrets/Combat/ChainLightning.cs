using System.Collections.Generic;
using UnityEngine;
using Code.Entities;
using Code.Sounds;
using Core.GameEvent;
using RuddnjsPool;
using System.Threading.Tasks;
using Code.Events;

namespace Code.Combat
{
    [RequireComponent(typeof(LineRenderer))]
    public class ChainLightning : Projectile
    {
        [Header("Chain Settings")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float chainRadius = 10f;
        [field: SerializeField] public int maxChains { get; set; } = 4;
        [SerializeField] private float chainDelay = 0.1f;
        [SerializeField] private GameObject projectile;

        private readonly Vector3 positionOffset = new Vector3(0f, 1f, 0f);

        private readonly HashSet<Transform> _hitTargets = new HashSet<Transform>();
        private readonly List<Transform> _chainTargets = new List<Transform>();

        private LineRenderer _lineRenderer;
        private bool _canHit;
        private int _chainIndex;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
        }
        
        protected override void Update()
        {
            base.Update();
            
            if (!_lineRenderer.enabled || _chainTargets.Count == 0)
                return;

            // 유효한 타겟만 갱신
            int validCount = 0;
            for (int i = 0; i < _chainTargets.Count; i++)
            {
                Transform t = _chainTargets[i];
                if (t == null) continue;

                _lineRenderer.SetPosition(validCount, t.position + positionOffset);
                validCount++;
            }

            // positionCount를 유효한 타겟 수에 맞춰 동기화
            _lineRenderer.positionCount = validCount;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (!_canHit) return;
            if (!TryDealDamage(other.transform)) return;

            projectile.SetActive(false);

            // 첫 타겟 명중 시 라인 초기 설정
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, other.transform.position + positionOffset);
            _chainTargets.Clear();
            _chainTargets.Add(other.transform);

            _canHit = false;
            
            var sfxEvt = SoundsEvents.PlaySfxEvent.Init(transform.position, explosionSound);
            soundChannel.RaiseEvent(sfxEvt);
            _ = ChainTargetsAsync(other.transform);
        }

        private bool TryDealDamage(Transform target)
        {
            if (_hitTargets.Contains(target)) return false;

            bool isHit = damageCaster.CastDamage(_damageData, transform.position, transform.forward, attackData);
            if (!isHit) return false;

            _hitTargets.Add(target);
            return true;
        }

        private async Task ChainTargetsAsync(Transform firstTarget)
        {
            Vector3 currentPosition = firstTarget.position;

            while (_chainIndex < maxChains)
            {
                var nextTarget = FindNextTarget(currentPosition);
                if (nextTarget == null)
                    break;

                ApplyDamageToTarget(nextTarget);
                _chainIndex++;
                _chainTargets.Add(nextTarget);

                // 💡 positionCount 증가와 동시에 즉시 유효 좌표 세팅 (0,0,0 방지)
                _lineRenderer.positionCount = _chainTargets.Count;
                _lineRenderer.SetPosition(_chainIndex, nextTarget.position + positionOffset);

                currentPosition = nextTarget.position;

                // 다음 체인까지 대기
                await Awaitable.WaitForSecondsAsync(chainDelay);
            }

            // 체인 끝나면 약간 대기 후 풀 반환
            await Awaitable.WaitForSecondsAsync(chainDelay);
            _myPool.Push(this);
        }

        private Transform FindNextTarget(Vector3 position)
        {
            Collider[] hitColliders = Physics.OverlapSphere(position, chainRadius, targetLayer);
            Transform nearest = null;
            float minDist = float.MaxValue;

            foreach (var hit in hitColliders)
            {
                if (hit == null) continue;
                Transform t = hit.transform;
                if (_hitTargets.Contains(t)) continue;

                float dist = Vector3.Distance(position, t.position);
                if (dist < 0.1f || dist >= minDist) continue;

                nearest = t;
                minDist = dist;
            }

            return nearest;
        }

        private void ApplyDamageToTarget(Transform target)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(
                    _damageData,
                    target.position,
                    transform.position - target.position,
                    attackData,
                    _owner);
            }

            _hitTargets.Add(target);
        }

        public override void ResetItem()
        {
            _lineRenderer.enabled = false;
            _lineRenderer.positionCount = 0;
            _chainIndex = 0;
            _canHit = true;

            _hitTargets.Clear();
            _chainTargets.Clear();

            projectile.SetActive(true);
            base.ResetItem();
        }
    }
}
