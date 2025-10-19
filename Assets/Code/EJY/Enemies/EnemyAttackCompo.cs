using System;
using Code.Combat;
using Code.Core.StatSystem;
using Code.Enemies;
using Code.Entities;
using Core.GameEvent;
using UnityEngine;
using UnityEngine.Events;

namespace Code.EJY.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] protected GameEventChannelSO effectChannel;
        [SerializeField] protected AttackDataSO attackData;
        [SerializeField] protected StatSO damageStat;
        [SerializeField] protected float attackInterval = 0.5f;
        
        protected Enemy _enemy;
        protected DamageCompo _damageCompo;
        protected EntityStatCompo _statCompo;
        protected EntityAnimatorTrigger _trigger;
        protected TargetDetector _detector;
        
        protected float _lastAttackTime;
        
        public bool CanAttack => Time.time - _lastAttackTime > attackInterval;
        public float AttackInterval => attackInterval;

        public UnityEvent OnAttackEvent;
        
        public virtual void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _damageCompo = entity.GetCompo<DamageCompo>();
            _trigger = entity.GetCompo<EntityAnimatorTrigger>();
            _detector = entity.GetCompo<TargetDetector>();
        }

        public virtual void AfterInitialize()
        {
            _trigger.OnAttackEventTrigger += HandleOnAttackEvent;
        }

        protected virtual void OnDestroy()
        {
            _trigger.OnAttackEventTrigger -= HandleOnAttackEvent;
        }

        public virtual void Attack()
        {
            _lastAttackTime = Time.time;
        }

        private void HandleOnAttackEvent() => OnAttackEvent?.Invoke();
    }
}