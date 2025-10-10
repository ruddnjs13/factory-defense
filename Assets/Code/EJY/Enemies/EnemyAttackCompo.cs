using System;
using Code.Combat;
using Code.Core.StatSystem;
using Code.Enemies;
using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] protected AttackDataSO attackData;
        [SerializeField] protected StatSO damageStat;
        [SerializeField] protected float attackInterval = 0.5f;
        [SerializeField] protected float attackRange = 1.5f;
        [SerializeField] private float detectRange = 3.5f;
        
        protected Enemy _enemy;
        protected DamageCompo _damageCompo;
        protected EntityStatCompo _statCompo;
        protected EntityAnimatorTrigger _trigger;
        protected NavMovement _movement;
        
        protected float _lastAttackTime;
        
        private Collider[] _hits;
        
        public Transform CurrentTarget { get; private set; }
        public NotifyValue<bool> IsTargeting { get; private set; } = new NotifyValue<bool>(false);
        public bool InAttackRange { get; private set; }

        public bool CanAttack => Time.time - _lastAttackTime < attackInterval;
        
        public virtual void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _damageCompo = entity.GetCompo<DamageCompo>();
            _trigger = entity.GetCompo<EntityAnimatorTrigger>();
            _movement = entity.GetCompo<NavMovement>();
            _hits = new Collider[1];
            IsTargeting.OnValueChanged += HandleTargetingChanged;

        }

        public virtual void AfterInitialize()
        {
        }

        private void OnDestroy()
        {
            IsTargeting.OnValueChanged -= HandleTargetingChanged;
        }
        
        private void HandleTargetingChanged(bool prev, bool next)
        {
            _movement.SetDestination(next ? CurrentTarget.position : _enemy.TargetTrm.position);
        }

        public virtual void Attack()
        {
            _lastAttackTime = Time.time;
        }

        protected virtual void FixedUpdate()
        {
            Array.Clear(_hits, 0, 1);
            Physics.OverlapSphereNonAlloc(_enemy.transform.position, detectRange, _hits, whatIsTarget);
            
            CurrentTarget =_hits[0]?.transform;
            IsTargeting.Value = CurrentTarget != null;
            
            int amount = Physics.OverlapSphereNonAlloc(_enemy.transform.position, attackRange, _hits, whatIsTarget);

            InAttackRange = amount > 0;
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