using System;
using Code.Entities;
using Code.Events;
using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public abstract class Enemy : Entity, IPoolable
    {
        [field: SerializeField] public Transform TargetTrm { get; private set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }
        [SerializeField] private PoolingItemSO deadBombItem;
        [SerializeField] private GameEventChannelSO effectChannel;
        [SerializeField] private Transform deadBombTrmPosition;
        
        public void SetTarget(Transform targetTrm) => TargetTrm = targetTrm;
        public GameObject GameObject => gameObject;
        
        private Pool _myPool;
        private EntityAnimatorTrigger _trigger;
        private int _deadBodyLayer;
        
        protected override void Awake()
        {
            base.Awake();
            _trigger = GetCompo<EntityAnimatorTrigger>();
            _trigger.OnDeadTrigger += OnDeadInAnimation;
            _deadBodyLayer = LayerMask.NameToLayer("DeadBody");
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public virtual void Init(Transform targetTrm)   
        {
            SetTarget(targetTrm);
        }

        private void OnDestroy()
        {
            _trigger.OnDeadTrigger -= OnDeadInAnimation;
        }

        public void ResetItem()
        {
            
        }

        private void OnDeadInAnimation()
        {
            effectChannel.RaiseEvent(EffectEvents.PlayPoolEffect.Initializer(deadBombTrmPosition.position, Quaternion.identity, deadBombItem, 10f));
            _myPool.Push(this);
        }

        public virtual void SetDead()
        {
            gameObject.layer = _deadBodyLayer;

        }
    }
}