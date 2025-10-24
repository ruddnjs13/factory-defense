using Chipmunk.ComponentContainers;
using Chipmunk.GameEvents;
using Code.Entities;
using Code.LKW.Building;
using Code.LKW.GameEvents;
using Code.SHS.Machines.Events;
using Code.SHS.TickSystem;
using Code.SHS.Worlds;
using EPOOutline;
using UnityEngine;

namespace Code.SHS.Machines
{
    [RequireComponent(typeof(ComponentContainer), typeof(Outlinable))]
    public abstract class BaseMachine : Entity, IContainerComponent, IMachine, ISelectable, ITick
    {
        [field: SerializeField] public MachineSO MachineSo { get; private set; }
        public ComponentContainer ComponentContainer { get; set; }
        public Vector2Int Position { get; private set; }
        public Vector2Int Size => MachineSo.size;

        [field:SerializeField] public BuildingInfoSO BuildingInfo { get; private set; }
        private Outlinable _outlineable;

        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
            Initialize();
        }


        protected override void Awake()
        {
            base.Awake();
            _outlineable = GetComponent<Outlinable>();
            _outlineable.enabled = false;
        }

        protected virtual void Initialize()
        {
            Debug.Assert(MachineSo != null, "MachineSo is not assigned");
            TickManager.RegisterTick(this);
            Position = Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));

            EventBus.Subscribe<MachineConstructedEvent>(MachineConstructHandler);
        }

        /// <summary>
        /// Size만큼의 모든 타일에서 이 Machine을 제거
        /// </summary>  
        private void ClearTiles(Vector2Int position, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y) + MachineSo.offset;
                    GridTile tile = WorldGrid.Instance.GetTile(tilePos);
                    if (tile.Machine == this)
                    {
                        tile.Machine = null;
                        WorldGrid.Instance.SetTile(tilePos, tile);
                    }
                }
            }
        }

        public virtual void OnTick(float deltaTime)
        {
        }

        protected virtual void MachineConstructHandler(MachineConstructedEvent evt)
        {
        }

        public virtual void OnDestroy()
        {
            TickManager.UnregisterTick(this);
            EventBus.Unsubscribe<MachineConstructedEvent>(MachineConstructHandler);
            if (WorldGrid.Instance == null) return;

            // Size만큼의 모든 타일 해제
            ClearTiles(Position, Size);
        }

        #region Selectable

        public virtual void Select()
        {
            EventBus<BuildingSelectedEvent>.Raise(new BuildingSelectedEvent(this));
            _outlineable.enabled = true;
        }

        public virtual void DeSelect()
        {
            EventBus<BuildingDeselectEvent>.Raise(new BuildingDeselectEvent());
            _outlineable.enabled = false;
        }

        #endregion
    }
}