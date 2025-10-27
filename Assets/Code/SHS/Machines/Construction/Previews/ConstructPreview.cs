using System;
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
        public MachineSO MachineSO { get; private set; }
        private MachineConstructor constructor;
        private Material previewMaterial;
        [SerializeField] private Material cannotPlaceMaterial;
        private MeshRenderer[] meshRenderers;

        private void Awake()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            // previewMaterial = meshRenderers[0].material;
        }

        public virtual void Initialize(MachineSO machineSO, MachineConstructor constructor)
        {
            MachineSO = machineSO;
            this.constructor = constructor;
            // Material targetMaterial = CanPlaceMachine(
            //     Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z)))
            //     ? previewMaterial
            //     : cannotPlaceMaterial;
            // foreach (MeshRenderer meshRenderer in meshRenderers)
            // {
            //     meshRenderer.material = previewMaterial;
            // }
        }

        /// <summary>
        /// 해당 위치에 Size만큼 기계를 배치할 수 있는지 확인
        /// </summary>
        protected bool CanPlaceMachine(Vector2Int position)
        {
            for (int x = 0; x < MachineSO.size.x; x++)
            {
                for (int y = 0; y < MachineSO.size.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y) + MachineSO.offset;
                    GridTile tile = WorldGrid.Instance.GetTile(tilePos);
                    if (tile.Machine != null)
                    {
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
            if (!CanPlaceMachine(position))
            {
                Debug.LogError(
                    $"Cannot place machine at {transform.position}. One or more tiles are already occupied.");
                return;
            }

            BaseMachine machineInstance = CreateInstance();
            WorldGrid.Instance.InstallMachineAt(position, machineInstance);
            if (machineInstance != null)
            {
                EventBus.Raise(new ResourceEvent(-MachineSO.cost));
            }
        }
    }
}