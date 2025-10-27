using System;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.TickSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines.Ports
{
    public abstract class BasePort : MonoBehaviour, IExcludeContainerComponent, IHasResource, ITick
    {
        public ShapeResource Resource { get; protected set; }
        public BaseMachine Machine { get; protected set; }
        public Vector2Int Position { get; protected set; }
        [field: SerializeField] public float Interval { get; private set; } = 1f;
        [SerializeField] private Direction localDirection;
        protected Direction Direction { get; private set; }

        public float Timer { get; protected set; } = 0f;
        public ComponentContainer ComponentContainer { get; set; }
        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
            Position = Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));

            float yRotation = transform.eulerAngles.y;
            Direction = localDirection.Rotate(yRotation);
            Machine = this.Get<BaseMachine>(true);
            Debug.Assert(Machine != null, $"can not find machine component in {componentContainer.gameObject.name}");
            TickManager.RegisterTick(this);
        }

        protected virtual void OnDestroy()
        {
            TickManager.UnregisterTick(this);
        }

        public virtual void OnTick(float deltaTime)
        {
            if (Resource == null) return;

            if (Timer >= Interval)
                OnPortTransferComplete();
            else
                Timer += deltaTime;
        }

        protected abstract void OnPortTransferComplete();

        private void OnDrawGizmosSelected()
        {
            Vector3 startPos = transform.position + Vector3.up * 0.1f;
            Vector3 directionVector = localDirection.Rotate(transform.eulerAngles.y).ToVector3();
            Gizmos.color = Color.white;
            Gizmos.DrawLine(startPos, startPos + directionVector);
            Gizmos.DrawSphere(startPos + directionVector, 0.1f);
        }
    }
}