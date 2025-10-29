using System;
using System.Threading.Tasks;
using Chipmunk.GameEvents;
using Chipmunk.Player;
using Chipmunk.Player.Events;
using Code.SHS.Machines.Events;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class ConstructPreview : MonoBehaviour
    {
        public MachineSO MachineSO { get; protected set; }
        protected MachineConstructor constructor;
        private Material previewMaterial;
        [SerializeField] private Material cannotPlaceMaterial;
        private MeshRenderer[] meshRenderers;

        public Vector2Int Position { get; private set; }

        private void Awake()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            // previewMaterial = meshRenderers[0].material;
        }

        public virtual void Initialize(MachineSO machineSO, MachineConstructor constructor, Vector2Int position)
        {
            MachineSO = machineSO;
            this.constructor = constructor;
            Position = position + machineSO.offset;
            // Material targetMaterial = CanPlaceMachine(
            //     Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z)))
            //     ? previewMaterial
            //     : cannotPlaceMaterial;
            // foreach (MeshRenderer meshRenderer in meshRenderers)
            // {
            //     meshRenderer.material = previewMaterial;
            // }
        }

        public void UpdatePosition(Vector2Int newPosition)
        {
            Position = newPosition + MachineSO.offset;
            transform.position = new Vector3(newPosition.x, 0f, newPosition.y);
        }

        /// <summary>
        /// 해당 위치에 Size만큼 기계를 배치할 수 있는지 확인
        /// </summary>
        protected bool CanPlaceMachine()
        {
            for (int x = 0; x < MachineSO.size.x; x++)
            {
                for (int y = 0; y < MachineSO.size.y; y++)
                {
                    Vector2Int tilePos = Position + new Vector2Int(x, y);
                    GridTile tile = WorldGrid.Instance.GetTile(tilePos);
                    if (tile.Machine != null)
                    {
                        Debug.Log("Checking tile at position: " + tilePos);
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual BaseMachine CreateInstance()
        {
            Debug.Assert(MachineSO.machinePrefab != null, "Machine prefab is not assigned in MachineSO.");
            BaseMachine machine = Instantiate(MachineSO.machinePrefab, transform.position, transform.rotation)
                .GetComponent<BaseMachine>();
            return machine;
        }

        public void TryConstruct()
        {
            if (PlayerResource.Instance.HasEnoughResource(MachineSO.cost) == false)
            {
                Debug.Log("Not enough resources to construct the machine.");
                return;
            }

            Vector2Int position = Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));
            if (!CanPlaceMachine())
            {
                Debug.LogError(
                    $"Cannot place machine at {Position}. One or more tiles are already occupied.");
                Debug.Log(WorldGrid.Instance.GetTile(position).Machine);
                return;
            }

            BaseMachine machineInstance = CreateInstance();
            if (machineInstance != null)
                EventBus.Raise(new ResourceEvent(-MachineSO.cost));
            WorldGrid.Instance.InstallMachineAt(position, machineInstance);
        }
    }
}