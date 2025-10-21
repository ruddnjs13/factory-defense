using System;
using Chipmunk.ComponentContainers;
using Chipmunk.GameEvents;
using Code.Entities;
using Code.LKW.Building;
using Code.LKW.GameEvents;
using Code.SHS.Machines.Events;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.TickSystem;
using Code.SHS.Worlds;
using EPOOutline;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.SHS.Machines
{
    [RequireComponent(typeof(ComponentContainer), typeof(Outlinable))]
    public abstract class BaseMachine : Entity, IContainerComponent, IMachine, ISelectable
    {
        [field: SerializeField] public MachineSO MachineSo { get; private set; }
        public ComponentContainer ComponentContainer { get; set; }
        public Vector2Int Position { get; private set; }
        [field: SerializeField] public Vector2Int Size => MachineSo.size;

        [SerializeField] private BuildingInfoSO buildingInfo;
        private Outlinable _outlinable;

        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
            Construct();
        }

        protected override void Awake()
        {
            base.Awake();
            _outlinable = GetComponent<Outlinable>();
            _outlinable.enabled = false;
        }

        protected virtual void Construct()
        {
            Debug.Assert(MachineSo != null, "MachineSo is not assigned");
            TickManager.RegisterTick(this);
            Position = Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));

            // Size만큼의 모든 타일이 비어있는지 확인
            if (!CanPlaceMachine(Position))
            {
                Debug.LogError($"Cannot place machine at {transform.position}. One or more tiles are already occupied.",
                    this);
                Destroy(gameObject);
                return;
            }

            // Size만큼의 모든 타일에 Machine 설정
            OccupyTiles(Position);

            EventBus.Raise(new MachineConstructEvent(this));
            this.OnWorldPlaced();

            EventBus.Subscribe<MachineConstructEvent>(MachineConstructHandler);
        }

        /// <summary>
        /// 해당 위치에 Size만큼 기계를 배치할 수 있는지 확인
        /// </summary>
        private bool CanPlaceMachine(Vector2Int position)
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y) + MachineSo.offset;
                    WorldTile tile = WorldGrid.Instance.GetTile(tilePos);
                    if (tile.Machine != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Size만큼의 모든 타일을 이 Machine으로 설정
        /// </summary>
        private void OccupyTiles(Vector2Int position)
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y) + MachineSo.offset;
                    WorldTile tile = WorldGrid.Instance.GetTile(tilePos);
                    tile.Machine = this;
                    WorldGrid.Instance.SetTile(tilePos, tile);
                }
            }
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
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    WorldTile tile = WorldGrid.Instance.GetTile(tilePos);
                    if (tile.Machine == this)
                    {
                        tile.Machine = null;
                        WorldGrid.Instance.SetTile(tilePos, tile);
                    }
                }
            }
        }

        protected virtual void OnWorldPlaced()
        {
        }

        public virtual void OnTick(float deltaTime)
        {
        }

        protected virtual void MachineConstructHandler(MachineConstructEvent evt)
        {
        }

        public virtual void OnDestroy()
        {
            TickManager.UnregisterTick(this);
            EventBus.Unsubscribe<MachineConstructEvent>(MachineConstructHandler);
            if (WorldGrid.Instance == null) return;

            // Size만큼의 모든 타일 해제
            ClearTiles(Position, Size);
        }

        #region Selectable
        
        public virtual void Select()
        {
            EventBus<BuildingSelectedEvent>.Raise(new BuildingSelectedEvent(this));
            _outlinable.enabled = true;
        }

        public virtual void DeSelect()
        {
            EventBus<BuildingDeselectEvent>.Raise(new BuildingDeselectEvent());
            _outlinable.enabled = false;
        }
        #endregion
    }
}