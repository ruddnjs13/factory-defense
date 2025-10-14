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
        [SerializeField] protected AttackDataSO attackData;
        [SerializeField] protected StatSO damageStat;
        [SerializeField] protected float attackInterval = 0.5f;
        
        protected Enemy _enemy;
        protected DamageCompo _damageCompo;
        protected EntityStatCompo _statCompo;
        protected EntityAnimatorTrigger _trigger;
        
        protected float _lastAttackTime;
        
        public bool CanAttack => Time.time - _lastAttackTime < attackInterval;
        
        public virtual void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _damageCompo = entity.GetCompo<DamageCompo>();
            _trigger = entity.GetCompo<EntityAnimatorTrigger>();
            

        }

        public virtual void AfterInitialize()
        {
        }

        public virtual void Attack()
        {
            _lastAttackTime = Time.time;
        }

        
    }
}