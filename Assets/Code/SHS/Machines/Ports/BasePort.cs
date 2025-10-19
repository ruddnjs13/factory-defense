using System;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.TickSystem;
using UnityEngine;

namespace Code.SHS.Machines.Ports
{
    public abstract class BasePort : MonoBehaviour, IExcludeContainerComponent, IAfterInitialze, IHasResource, ITick
    {
        public ShapeResource Resource { get; protected set; }
        public BaseMachine Machine { get; protected set; }
        public Vector2Int Position { get; protected set; }
        [field: SerializeField] public float Interval { get; private set; } = 1f;

        public float Timer { get; protected set; } = 0f;
        public ComponentContainer ComponentContainer { get; set; }

        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
            Machine = this.Get<BaseMachine>(true);
            Debug.Assert(Machine != null, $"can not find machine component in {componentContainer.gameObject.name}");
            TickManager.RegisterTick(this);
        }

        protected virtual void OnDestroy()
        {
            TickManager.UnregisterTick(this);
        }

        public void AfterInitialze()
        {
            Position = Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));
        }

        public virtual void OnTick(float deltaTime)
        {
            if (Resource == null) return;

            if (Timer >= Interval)
                OnPortTransferComplete();
            Timer += deltaTime;
        }

        protected abstract void OnPortTransferComplete();
    }
}