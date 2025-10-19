using System;
using System.Collections.Generic;
using Chipmunk.GameEvents;
using Chipmunk.Player;
using Code.SHS.Machines.Events;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines.Construction
{
    public class MachineConstructor : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private PlayerInputReader playerInput;
        private MachineSO machineSO;
        private GameObject mainPreview;
        private Dictionary<Vector3, GameObject> previewInstances = new Dictionary<Vector3, GameObject>();
        private bool isLeftClicking = false;

        private void Awake()
        {
            EventBus<MachineSelectEvent>.OnEvent += OnMachineSelect;
            playerInput.OnLeftClickEvent += LeftClickHandler;
            playerInput.OnRightClickEvent += RightClickHandler;
            playerInput.OnRotateEvent += RotateHandler;
            playerInput.OnMiddleClickEvent += MiddleClickHandler;
        }

        private void OnDestroy()
        {
            EventBus<MachineSelectEvent>.OnEvent -= OnMachineSelect;
            playerInput.OnLeftClickEvent -= LeftClickHandler;
            playerInput.OnRightClickEvent -= RightClickHandler;
            playerInput.OnRotateEvent -= RotateHandler;
            playerInput.OnMiddleClickEvent -= MiddleClickHandler;
        }

        private void OnMachineSelect(MachineSelectEvent evt)
        {
            machineSO = evt.MachineSo;
            if (mainPreview != null)
            {
                Destroy(mainPreview);
            }
            ClearPreviews();

            mainPreview = Instantiate(machineSO.machineGhostPrefab);
        }

        private void Update()
        {
            if (mainPreview != null)
            {
                if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
                {
                    Vector3 position = Vector3Int.RoundToInt(hit.point) * new Vector3Int(1, 0, 1);
                    mainPreview.transform.position = position;
                    
                    if (isLeftClicking)
                    {
                        AddPreviewAtPosition(position);
                    }
                }
            }
        }

        private void AddPreviewAtPosition(Vector3 position)
        {
            // 이미 해당 위치에 Preview가 있는지 확인
            if (previewInstances.ContainsKey(position))
                return;

            // Size만큼의 모든 타일이 비어있는지 확인
            if (!CanPlaceMachineAtPosition(position))
                return;

            GameObject preview = Instantiate(machineSO.machineGhostPrefab, position, mainPreview.transform.rotation);
            previewInstances.Add(position, preview);
        }

        /// <summary>
        /// 해당 위치에 Size만큼 기계를 배치할 수 있는지 확인
        /// </summary>
        private bool CanPlaceMachineAtPosition(Vector3 position)
        {
            Vector2Int basePos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
            Vector2Int size = machineSO.size;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = basePos + new Vector2Int(x, y);
                    WorldTile tile = WorldGrid.Instance.GetTile(tilePos);
                    if (tile.Machine != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void LeftClickHandler(bool isPressed)
        {
            isLeftClicking = isPressed;
        }

        private void MiddleClickHandler(bool isPressed)
        {
            if (isPressed && machineSO != null && previewInstances.Count > 0)
            {
                ConstructAll();
            }
        }

        private void ConstructAll()
        {
            int totalCost = machineSO.cost * previewInstances.Count;

            // 비용 체크
            if (Portal.resourceCount < totalCost)
            {
                // 설치 가능한 만큼만 설치
                int affordableCount = Portal.resourceCount / machineSO.cost;
                if (affordableCount == 0)
                {
                    return;
                }
                
                int count = 0;
                foreach (var kvp in previewInstances)
                {
                    if (count >= affordableCount)
                        break;
                    
                    TryConstructSingle(kvp.Key, kvp.Value.transform.rotation);
                    count++;
                }
            }
            else
            {
                // 모두 설치
                foreach (var kvp in previewInstances)
                {
                    TryConstructSingle(kvp.Key, kvp.Value.transform.rotation);
                }
            }

            ClearPreviews();
        }

        private bool TryConstructSingle(Vector3 position, Quaternion rotation)
        {
            // Size만큼의 모든 타일이 비어있는지 다시 확인
            if (!CanPlaceMachineAtPosition(position))
                return false;

            if (Portal.resourceCount < machineSO.cost) 
                return false;

            Portal.resourceCount -= machineSO.cost;
            Instantiate(machineSO.machinePrefab, position, rotation);
            return true;
        }

        private void ClearPreviews()
        {
            foreach (var preview in previewInstances.Values)
            {
                if (preview != null)
                    Destroy(preview);
            }
            previewInstances.Clear();
        }

        private void RightClickHandler(bool isPressed)
        {
            if (isPressed && machineSO != null)
            {
                CancelConstruction();
            }
        }

        private void RotateHandler()
        {
            if (mainPreview != null)
            {
                mainPreview.transform.Rotate(Vector3.up, 90f);
                
                // 기존 Preview들도 회전
                foreach (var preview in previewInstances.Values)
                {
                    if (preview != null)
                        preview.transform.Rotate(Vector3.up, 90f);
                }
            }
        }

        private void CancelConstruction()
        {
            machineSO = null;
            if (mainPreview != null)
            {
                Destroy(mainPreview);
            }
            ClearPreviews();
        }
    }
}