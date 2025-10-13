using System;
using Chipmunk.ComponentContainers;
using Chipmunk.GameEvents;
using Code.Entities;
using Code.SHS.Machines.Events;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines
{
    [RequireComponent(typeof(ComponentContainer))]
    public abstract class BaseMachine : Entity, IContainerComponent, IMachine
    {
        [field: SerializeField] public MachineSO MachineSo { get; private set; }
        public ComponentContainer ComponentContainer { get; set; }
        public Vector2Int Position { get; private set; }
        [field: SerializeField] public Vector2Int Size { get; private set; }

        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
            TickManager.RegisterTick(this);
            Position = Vector2Int.FloorToInt(new Vector2(transform.position.x, transform.position.z));
            WorldTile worldTile = WorldGrid.Instance.GetTile(Position);
            if (worldTile.Machine == null)
            {
                worldTile.Machine = this;
                WorldGrid.Instance.SetTile(Position, worldTile);
                EventBus.Raise(new MachineConstructEvent(this));
                this.OnWorldPlaced();
            }
            else
            {
                Debug.LogError($"Tile at {transform.position} is already occupied by another machine.", this);
                Destroy(gameObject);
            }

            EventBus.Subscribe<MachineConstructEvent>(MachineConstructHandler);
        }

        protected virtual void OnWorldPlaced()
        {
        }

        public virtual void OnTick(float deltaTime)
        {
        }

        public virtual void Update()
        {
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
    }
}