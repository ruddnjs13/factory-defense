using System;
using Chipmunk.ComponentContainers;
using Chipmunk.GameEvents;
using Code.SHS.Machines.Events;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines
{
    public abstract class BaseMachine : MonoBehaviour, IContainerComponent
    {
        [field: SerializeField] public MachineSO MachineSo { get; private set; }
        public ComponentContainer ComponentContainer { get; set; }
        public Vector2Int Position { get; private set; }

        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
            Position = Vector2Int.FloorToInt(new Vector2(transform.position.x, transform.position.z));
            WorldTile worldTile = WorldGrid.Instance.GetTile(Position);
            if (worldTile.Machine == null)
            {
                worldTile.Machine = this;
                WorldGrid.Instance.SetTile(Position, worldTile);
                EventBus.Raise(new MachineConstructEvent(this));
            }
            else
            {
                Debug.LogError($"Tile at {transform.position} is already occupied by another machine.", this);
                Destroy(gameObject);
            }

            EventBus.Subscribe<MachineConstructEvent>(MachineConstructHandler);
        }

        protected virtual void MachineConstructHandler(MachineConstructEvent evt)
        {
        }

        public virtual void OnDestroy()
        {
            EventBus.Unsubscribe<MachineConstructEvent>(MachineConstructHandler);
            WorldTile worldTile = WorldGrid.Instance.GetTile(Position);
            if (worldTile.Machine == this)
            {
                worldTile.Machine = null;
            }
        }

        public virtual void Update()
        {
        }
    }
}