using System;
using System.Collections.Generic;
using Chipmunk.GameEvents;
using Chipmunk.Player;
using Code.SHS.Machines.Construction.Previews;
using Code.SHS.Machines.Events;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines.Construction
{
    public class MachineConstructor : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private PlayerInputReader playerInput;
        private ConstructPreview mainPreview;
        private List<ConstructPreview> previewInstances = new();

        public Dictionary<Vector2Int, ConstructPreview> previewByPosition { get; private set; } =
            new Dictionary<Vector2Int, ConstructPreview>();

        private bool isLeftClicking = false;
        private bool removeMode = false;
        private Vector2Int previousPosition = Vector2Int.zero;
        [SerializeField] private Transform containerTransform;
        [SerializeField] private Transform destroyRegion;

        private void Awake()
        {
            EventBus<MachineSelectEvent>.OnEvent += OnMachineSelect;
            playerInput.OnLeftClickEvent += LeftClickHandler;
            playerInput.OnRightClickEvent += RightClickHandler;
            playerInput.OnRotateEvent += RotateHandler;
            playerInput.OnMiddleClickEvent += MiddleClickHandler;

            containerTransform.SetParent(null);
            containerTransform.transform.rotation = Quaternion.Euler(90, 0, 0);
            containerTransform.gameObject.SetActive(false);
            destroyRegion.SetParent(null);
            destroyRegion.transform.rotation = Quaternion.Euler(0, 0, 0);
            destroyRegion.gameObject.SetActive(false);
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
            if (mainPreview != null)
                Destroy(mainPreview.gameObject);

            MachineSO machine = evt.MachineSo;
            mainPreview = Instantiate(machine.machinePreviewPrefab, Vector3.zero, machine.rotation)
                .GetComponent<ConstructPreview>();
            mainPreview.Initialize(machine, this);
            Debug.Assert(mainPreview != null, "ConstructPreview component not found on machinePreviewPrefab");

            containerTransform.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (mainPreview != null && removeMode == false)
            {
                if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
                {
                    Vector2Int position = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
                    if (position != previousPosition)
                    {
                        if (isLeftClicking)
                        {
                            Vector2Int directionVector =
                                (position - previousPosition);
                            directionVector.Clamp(-Vector2Int.one, Vector2Int.one);
                            Direction direction = directionVector.ToDirection();
                            Quaternion rotation = direction.ToQuaternion();

                            // 월드 좌표계 direction을 mainPreview의 로컬 좌표계로 변환
                            Direction localDirection = direction.ToLocalDirection(mainPreview.transform.rotation)
                                .Rotate(mainPreview.MachineSO.rotation.eulerAngles.y);
                            AddPreviewAtPosition(previousPosition, localDirection);
                            mainPreview.transform.rotation = rotation * mainPreview.MachineSO.rotation;
                        }

                        mainPreview.transform.position = new Vector3(position.x, 0f, position.y);
                        containerTransform.position = new Vector3(position.x, 0f, position.y);
                        previousPosition = position;
                    }
                }
            }

            if (removeMode)
            {
                if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
                {
                    Vector2Int position =
                        new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));

                    Vector3 middlePoint = new Vector3(
                        (previousPosition.x + position.x) / 2f,
                        0f,
                        (previousPosition.y + position.y) / 2f);
                    destroyRegion.position = middlePoint;
                    Vector3 scale = new Vector3(
                        Math.Abs(previousPosition.x - position.x) + 1f,
                        1f,
                        Math.Abs(previousPosition.y - position.y) + 1f);
                    destroyRegion.localScale = scale;
                    destroyRegion.gameObject.SetActive(true);
                }
            }
        }

        private void AddPreviewAtPosition(Vector2Int gridPosition, Direction nextDirection)
        {
            Vector2Int size = mainPreview.MachineSO.size;

            // Size만큼의 모든 타일이 비어있는지 확인
            if (!CanPlaceMachineAtPosition(gridPosition))
                return;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = gridPosition + new Vector2Int(x, y) + mainPreview.MachineSO.offset;
                    DestroyPreviewAt(tilePos);
                }
            }

            ConstructPreview preview = Instantiate(mainPreview);
            preview.Initialize(mainPreview.MachineSO, this);
            preview.SetNextDirection(nextDirection);
            previewInstances.Add(preview);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = gridPosition + new Vector2Int(x, y) + mainPreview.MachineSO.offset;
                    previewByPosition.Add(tilePos, preview);
                }
            }
        }

        /// <summary>
        /// 해당 위치에 Size만큼 기계를 배치할 수 있는지 확인
        /// </summary>
        private bool CanPlaceMachineAtPosition(Vector2Int gridPosition)
        {
            Vector2Int size = mainPreview.MachineSO.size;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = gridPosition + new Vector2Int(x, y);
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
            if (isPressed)
            {
                if (playerInput.ShiftKeyPressed)
                {
                    if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
                    {
                        previousPosition =
                            new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));

                        removeMode = true;
                    }
                }
            }
            else
            {
                if (mainPreview != null && removeMode == false)
                    AddPreviewAtPosition(previousPosition, Direction.None);
                if (removeMode)
                {
                    destroyRegion.gameObject.SetActive(false);
                    removeMode = false;

                    if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
                    {
                        Vector2Int position =
                            new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
                        int minX = Math.Min(previousPosition.x, position.x);
                        int maxX = Math.Max(previousPosition.x, position.x);
                        int minY = Math.Min(previousPosition.y, position.y);
                        int maxY = Math.Max(previousPosition.y, position.y);
                        ;
                        for (int x = minX; x <= maxX; x++)
                        {
                            for (int y = minY; y <= maxY; y++)
                            {
                                Vector2Int tilePos = new Vector2Int(x, y);
                                DestroyPreviewAt(tilePos);
                                // DestroyPreviewAt(tilePos);
                                Destroy(WorldGrid.Instance.GetTile(tilePos).Machine?.gameObject);
                            }
                        }
                    }
                }
            }
        }

        private void MiddleClickHandler(bool isPressed)
        {
            if (!isPressed)
            {
                ConstructAll();
            }
        }

        private void ConstructAll()
        {
            containerTransform.gameObject.SetActive(false);
            foreach (ConstructPreview constructPreview in previewInstances)
            {
                constructPreview.TryConstruct();
            }

            ClearPreviews();
        }

        private void ClearPreviews()
        {
            containerTransform.gameObject.SetActive(false);
            foreach (var preview in previewInstances)
            {
                if (preview != null)
                    Destroy(preview.gameObject);
            }

            previewInstances.Clear();
            previewByPosition.Clear();
        }

        private void RightClickHandler(bool isPressed)
        {
            if (isPressed)
                CancelConstruction();
        }

        private void RotateHandler()
        {
            if (mainPreview != null)
            {
                mainPreview.transform.Rotate(Vector3.up, 90f);
            }
        }

        private void CancelConstruction()
        {
            if (mainPreview != null)
            {
                Destroy(mainPreview.gameObject);
                mainPreview = null;
            }

            ClearPreviews();
        }

        private void DestroyPreviewAt(Vector2Int gridPosition)
        {
            if (previewByPosition.TryGetValue(gridPosition, out ConstructPreview preview))
            {
                for (int x = 0; x < mainPreview.MachineSO.size.x; x++)
                {
                    for (int y = 0; y < mainPreview.MachineSO.size.y; y++)
                    {
                        Vector2Int tilePos = gridPosition + new Vector2Int(x, y) + mainPreview.MachineSO.offset;
                        previewByPosition.Remove(tilePos);
                    }
                }

                previewInstances.Remove(preview);
                Destroy(preview.gameObject);
            }
        }
    }
}