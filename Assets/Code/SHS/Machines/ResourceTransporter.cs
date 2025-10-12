using System;
using System.Collections.Generic;
using Code.SHS.Extensions;
using Code.SHS.Machines.Events;
using UnityEngine;

namespace Code.SHS.Machines
{
    public abstract class ResourceTransporter : BaseMachine, IInputResource, IOutputResource
    {
        [SerializeField] protected List<DirectionEnum> _inputDirection = new List<DirectionEnum>();
        [SerializeField] protected List<DirectionEnum> _outputDirection = new List<DirectionEnum>();
        public Action OnResourceChanged;
        protected List<BaseMachine> _connectedMachines = new List<BaseMachine>();
        public Resource? currentResource;
        protected GameObject resourceVisual;

        protected override void MachineConstructHandler(MachineConstructEvent evt)
        {
            base.MachineConstructHandler(evt);
            foreach (var dir in _outputDirection)
            {
                Vector2Int inputPosition = Position + Vector3Int.RoundToInt(Direction.GetDirection(dir)).ToXZ();
                if (evt.Machine.Position == inputPosition)
                {
                    _connectedMachines.Add(evt.Machine);
                    Debug.Log($"Connected machines: {_connectedMachines.Count}");
                }
            }
        }

        public virtual bool TryInsertResource(Resource resource, DirectionEnum inputDir)
        {
            if (!_inputDirection.Contains(inputDir)) return false;
            if (currentResource != null) return false;
            currentResource = resource;
            resourceVisual = Instantiate(resource.ResourceSo.prefab, transform);
            return true;
        }

        public virtual void ExtractResource()
        {
            if (currentResource == null) return;
            foreach (var dir in _outputDirection)
            {
                Vector2Int outputPosition = Position + Vector3Int.RoundToInt(Direction.GetDirection(dir)).ToXZ();
                foreach (var machine in _connectedMachines)
                {
                    Debug.Log(machine);
                    if (machine.Position == outputPosition && machine is IInputResource inputMachine)
                    {
                        if (inputMachine.TryInsertResource((Resource)currentResource, Direction.GetOpposite(dir)))
                        {
                            currentResource = null;
                            if (resourceVisual != null)
                            {
                                Destroy(resourceVisual);
                                resourceVisual = null;
                            }

                            return;
                        }
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (var dir in _inputDirection)
            {
                Vector3 directionVector = Direction.GetDirection(dir);
                Vector3 position = transform.position + directionVector + new Vector3(0f, 0.5f, 0f);
                Gizmos.DrawWireCube(position, Vector3.one * 1f);
            }

            Gizmos.color = Color.yellow;
            foreach (var dir in _outputDirection)
            {
                Vector3 directionVector = Direction.GetDirection(dir);
                Vector3 position = transform.position + directionVector + new Vector3(0f, 0.5f, 0f);
                Gizmos.DrawWireCube(position, Vector3.one * 1f);
            }
        }
    }
}