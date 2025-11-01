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
        [SerializeField] private Material previewMaterial;
        [SerializeField] private Material cannotPlaceMaterial;
        private Renderer[] meshRenderers = null;

        public Vector2Int Position { get; private set; }

        private void Awake()
        {
        }

        public virtual void Initialize(MachineSO machineSO, MachineConstructor constructor, Vector2Int position)
        {
            if (meshRenderers == null)
                meshRenderers = GetComponentsInChildren<Renderer>();

            MachineSO = machineSO;
            this.constructor = constructor;
            UpdatePosition(position);
        }

        public void UpdatePosition(Vector2Int newPosition)
        {
            Position = newPosition + MachineSO.offset;
            transform.position = new Vector3(newPosition.x, 0f, newPosition.y);

            Material targetMaterial = CanPlaceMachine()
                ? previewMaterial
                : cannotPlaceMaterial;
            foreach (Renderer meshRenderer in meshRenderers)
            {
                if (meshRenderer.materials.Length > 1)
                {
                    Material[] materials = meshRenderer.materials;
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i] = targetMaterial;
                    }

                    meshRenderer.materials = materials;
                }
                else
                {
                    meshRenderer.material = targetMaterial;
                }
            }
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
                    if (CheckCanPlaceAt(tilePos, tile) == false)
                        return false;
                }
            }

            return true;
        }

        protected virtual bool CheckCanPlaceAt(Vector2Int tilePos, GridTile tile)
        {
            if (tile.Machine != null || tile.Ground is ResourceGroundSO)
                return false;

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