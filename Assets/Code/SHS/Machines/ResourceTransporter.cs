using System;
using System.Collections.Generic;
using Chipmunk.ComponentContainers;
using Code.SHS.Extensions;
using Code.SHS.Machines.Events;
using Code.SHS.Machines.ResourceVisualizer;
using Code.SHS.Worlds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    public abstract class ResourceTransporter : BaseMachine, IInputResource, IOutputResource, ITransporter
    {
        [SerializeField] protected DirectionEnum inputDirection;
        [SerializeField] protected DirectionEnum outputDirection;

        [SerializeField] private BaseResourceVisualizer resourceVisualizer;
        public Resource? currentResource;

        private IInputResource linkedInputMachine;
        private IOutputResource linkedOutputMachine;


        [field: SerializeField] public float TransportInterval { get; private set; } = 1f;
        public Action<Resource> OnResourceInput { get; set; }
        public Action<Resource> OnResourceOutput { get; set; }
        private float transferTimer;

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);

            float yRotation = transform.eulerAngles.y;
            inputDirection = Direction.RotateDirection(inputDirection, yRotation);
            outputDirection = Direction.RotateDirection(outputDirection, yRotation);
        }

        protected override void MachineConstructHandler(MachineConstructEvent evt)
        {
            base.MachineConstructHandler(evt);
        }

        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            if (transferTimer >= TransportInterval)
                OutputItem();
            transferTimer += deltaTime;
        }

        public bool CanAcceptInputFrom(IOutputResource machine)
        {
            Vector2Int directionVec = Position - machine.Position;
            DirectionEnum direction = Direction.GetOpposite(Direction.GetDirection(directionVec));
            if (direction != inputDirection)
                return false;
            return true;
        }

        public virtual bool TryReceiveResource(IOutputResource machine, Resource resource)
        {
            if (currentResource != null) return false;
            ReceiveResource(resource);
            return true;
        }

        public virtual void ReceiveResource(Resource resource)
        {
            OnResourceInput?.Invoke(resource);
            currentResource = resource;
            resourceVisualizer.StartTransport(resource, TransportInterval);
            transferTimer = 0f;
        }

        public void OutputItem()
        {
            if (currentResource == null) return;
            transferTimer = 0f;
            Vector2Int outputPosition =
                Position + Vector3Int.RoundToInt(Direction.GetDirection(outputDirection)).ToXZ();
            WorldTile outputTile = WorldGrid.Instance.GetTile(outputPosition);
            BaseMachine machine = outputTile.Machine;
            if (machine != null && machine is IInputResource inputMachine)
            {
                if (inputMachine.CanAcceptInputFrom(this) && currentResource != null)
                {
                    if (inputMachine.TryReceiveResource(this, (Resource)currentResource))
                    {
                        OnResourceOutput?.Invoke((Resource)currentResource);
                        resourceVisualizer.EndTransport();
                        currentResource = null;
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 directionVector = Direction.GetDirection(inputDirection);
            Vector3 position = transform.position + directionVector + new Vector3(0f, 0.5f, 0f);
            Gizmos.DrawWireCube(position, Vector3.one * 1f);

            Gizmos.color = Color.yellow;
            directionVector = Direction.GetDirection(outputDirection);
            position = transform.position + directionVector + new Vector3(0f, 0.5f, 0f);
            Gizmos.DrawWireCube(position, Vector3.one * 1f);
        }
    }
}