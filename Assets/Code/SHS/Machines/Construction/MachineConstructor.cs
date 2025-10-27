using System;
using System.Collections.Generic;
using Chipmunk.GameEvents;
using Chipmunk.Player;
using Chipmunk.UI;
using Code.SHS.Machines.Construction.Previews;
using Code.SHS.Machines.Events;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines.Construction
{
    public class MachineConstructor : MonoBehaviour
    {
        private const float GROUND_HEIGHT = 0f;
        private const float ROTATION_ANGLE = 90f;

        [Header("Input")] [SerializeField] private PlayerInputReader playerInput;

        [Header("Raycast")] [SerializeField] private LayerMask layerMask;

        [Header("Transforms")] [SerializeField]
        private Transform previewContainer;

        [SerializeField] private Transform destroyRegion;

        private ConstructPreview mainPreview;
        private readonly List<ConstructPreview> previewInstances = new();
        private readonly Dictionary<Vector2Int, ConstructPreview> previewByPosition = new();

        private bool isLeftClicking;
        private bool isRemoveMode;
        private Direction previousDirection = Direction.None;
        private Vector2Int currentGridPosition;

        private void Awake()
        {
            InitializeTransforms();
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void Update()
        {
            if (isRemoveMode)
            {
                UpdateRemoveModePreview();
            }
            else if (mainPreview != null)
            {
                UpdateConstructPreview();
            }
        }

        private void InitializeTransforms()
        {
            previewContainer.SetParent(null);
            previewContainer.rotation = Quaternion.Euler(0, 0, 0);
            previewContainer.gameObject.SetActive(false);

            destroyRegion.SetParent(null);
            destroyRegion.rotation = Quaternion.identity;
            destroyRegion.gameObject.SetActive(false);
        }

        private void SubscribeEvents()
        {
            EventBus<MachineSelectEvent>.OnEvent += OnMachineSelect;
            playerInput.OnLeftClickEvent += OnLeftClick;
            playerInput.OnRightClickEvent += OnRightClick;
            playerInput.OnRotateEvent += OnRotate;
            playerInput.OnMiddleClickEvent += OnMiddleClick;
        }

        private void UnsubscribeEvents()
        {
            EventBus<MachineSelectEvent>.OnEvent -= OnMachineSelect;
            playerInput.OnLeftClickEvent -= OnLeftClick;
            playerInput.OnRightClickEvent -= OnRightClick;
            playerInput.OnRotateEvent -= OnRotate;
            playerInput.OnMiddleClickEvent -= OnMiddleClick;
        }

        private void OnMachineSelect(MachineSelectEvent evt)
        {
            if (mainPreview != null)
                Destroy(mainPreview.gameObject);

            MachineSO machine = evt.MachineSo;
            mainPreview = Instantiate(machine.machinePreviewPrefab, previewContainer.position, machine.rotation,
                    previewContainer)
                .GetComponent<ConstructPreview>();

            Debug.Assert(mainPreview != null, "ConstructPreview component not found on machinePreviewPrefab");

            mainPreview.Initialize(machine, this);
            previewContainer.gameObject.SetActive(true);
        }

        private void OnLeftClick(bool isPressed)
        {
            isLeftClicking = isPressed;
            if (UIPointerDetector.IsPointerInUI) return;

            if (isPressed)
            {
                HandleLeftClickPressed();
            }
            else
            {
                HandleLeftClickReleased();
            }
        }

        private void OnRightClick(bool isPressed)
        {
            if (isPressed)
            {
                CancelConstruction();
            }
        }

        private void OnRotate()
        {
            if (mainPreview != null)
            {
                mainPreview.transform.Rotate(Vector3.up, ROTATION_ANGLE);
            }
        }

        private void OnMiddleClick(bool isPressed)
        {
            if (!isPressed)
            {
                ConstructAll();
                CancelConstruction();
            }
        }

        private void UpdateConstructPreview()
        {
            if (!TryGetMouseGridPosition(out Vector2Int newPosition))
                return;

            if (newPosition == currentGridPosition)
                return;

            if (isLeftClicking)
            {
                PlacePreviewBetweenPositions(currentGridPosition, newPosition);
            }

            if (previewByPosition.TryGetValue(newPosition, out ConstructPreview existingPreview) &&
                existingPreview is ConveyorPreview conveyorPreview)
            {
                mainPreview.gameObject.SetActive(false);
            }
            else
            {
                mainPreview.gameObject.SetActive(true);
            }

            SetMainPreviewPosition(newPosition);
            currentGridPosition = newPosition;
        }

        private void UpdateRemoveModePreview()
        {
            if (!TryGetMouseGridPosition(out Vector2Int mousePosition))
                return;

            UpdateDestroyRegion(currentGridPosition, mousePosition);
        }

        private void PlacePreviewBetweenPositions(Vector2Int fromPosition, Vector2Int toPosition)
        {
            Vector2Int directionVector = toPosition - fromPosition;
            directionVector.Clamp(-Vector2Int.one, Vector2Int.one);
            Direction worldDirection = directionVector.ToDirection();


            if (worldDirection == Direction.None)
                return;

            Quaternion worldRotation = worldDirection.ToQuaternion();
            // Direction localDirection = worldDirection
            //     .ToLocalDirection(mainPreview.transform.rotation)
            //     .Rotate(mainPreview.MachineSO.rotation.eulerAngles.y);
            ConstructPreview existingPreview = null;
            if (previewByPosition.TryGetValue(toPosition, out existingPreview))
                if (existingPreview is ConveyorPreview conveyorPreview)
                    // conveyorPreview.AddInputDirection(worldDirection);
                    conveyorPreview.AddInputDirection(worldDirection.Opposite());

            if (previewByPosition.TryGetValue(fromPosition, out existingPreview))
            {
                if (existingPreview is ConveyorPreview conveyorPreview)
                    conveyorPreview.AddOutputDirection(worldDirection);
            }
            else
            {
                AddPreviewAtPosition(fromPosition, worldDirection);
            }


            mainPreview.transform.rotation = worldRotation * mainPreview.MachineSO.rotation;
        }

        private void AddPreviewAtPosition(Vector2Int gridPosition, Direction nextDirection)
        {
            if (!CanPlaceMachineAtPosition(gridPosition))
                return;

            RemoveOverlappingPreviews(gridPosition);

            ConstructPreview preview = Instantiate(mainPreview, mainPreview.transform.position,
                mainPreview.transform.rotation, null);
            preview.gameObject.SetActive(true);
            preview.Initialize(mainPreview.MachineSO, this);
            if (preview is ConveyorPreview conveyorPreview)
                conveyorPreview.AddOutputDirection(nextDirection);
            previewInstances.Add(preview);

            RegisterPreviewAtPosition(gridPosition, preview);
        }

        private void RemoveOverlappingPreviews(Vector2Int gridPosition)
        {
            Vector2Int size = mainPreview.MachineSO.size;
            Vector2Int offset = mainPreview.MachineSO.offset;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = gridPosition + new Vector2Int(x, y) + offset;
                    DestroyPreviewAt(tilePos);
                }
            }
        }

        private void RegisterPreviewAtPosition(Vector2Int gridPosition, ConstructPreview preview)
        {
            Vector2Int size = mainPreview.MachineSO.size;
            Vector2Int offset = mainPreview.MachineSO.offset;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = gridPosition + new Vector2Int(x, y) + offset;
                    previewByPosition[tilePos] = preview;
                }
            }
        }

        private bool CanPlaceMachineAtPosition(Vector2Int gridPosition)
        {
            Vector2Int size = mainPreview.MachineSO.size;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = gridPosition + new Vector2Int(x, y);
                    GridTile tile = WorldGrid.Instance.GetTile(tilePos);

                    if (tile.Machine != null)
                        return false;
                }
            }

            return true;
        }

        private void ConstructAll()
        {
            foreach (ConstructPreview preview in previewInstances)
            {
                if (preview != null)
                {
                    preview.TryConstruct();
                }
            }

            ClearPreviews();
        }

        private void ClearPreviews()
        {
            foreach (ConstructPreview preview in previewInstances)
            {
                if (preview != null)
                {
                    Destroy(preview.gameObject);
                }
            }

            previewInstances.Clear();
            previewByPosition.Clear();
        }

        private void CancelConstruction()
        {
            if (mainPreview != null)
            {
                Destroy(mainPreview.gameObject);
                mainPreview = null;
            }

            previewContainer.gameObject.SetActive(false);

            ClearPreviews();
        }

        private void DestroyPreviewAt(Vector2Int gridPosition)
        {
            if (!previewByPosition.TryGetValue(gridPosition, out ConstructPreview preview))
                return;

            UnregisterPreview(preview);
            previewInstances.Remove(preview);
            Destroy(preview.gameObject);
        }

        private void UnregisterPreview(ConstructPreview preview)
        {
            List<Vector2Int> positionsToRemove = new List<Vector2Int>();

            foreach (var kvp in previewByPosition)
            {
                if (kvp.Value == preview)
                {
                    positionsToRemove.Add(kvp.Key);
                }
            }

            foreach (Vector2Int pos in positionsToRemove)
            {
                previewByPosition.Remove(pos);
            }
        }

        private void HandleLeftClickPressed()
        {
            if (playerInput.ShiftKeyPressed && TryGetMouseGridPosition(out Vector2Int position))
            {
                currentGridPosition = position;
                isRemoveMode = true;
            }
        }

        private void HandleLeftClickReleased()
        {
            if (isRemoveMode)
            {
                ExecuteRemoveMode();
            }
            else if (mainPreview != null)
            {
                // if (previewByPosition.TryGetValue(currentGridPosition, out ConstructPreview existingPreview))
                                    // {
                                    //     if (existingPreview is ConveyorPreview conveyorPreview)
                                    //     {
                                    //         conveyorPreview.AddInputDirection(worldDirection.Opposite());
                                    //     }
                                    // }

                AddPreviewAtPosition(currentGridPosition, Direction.None);
            }
        }

        private void ExecuteRemoveMode()
        {
            destroyRegion.gameObject.SetActive(false);
            isRemoveMode = false;

            if (!TryGetMouseGridPosition(out Vector2Int endPosition))
                return;

            RemoveMachinesInArea(currentGridPosition, endPosition);
        }

        private void RemoveMachinesInArea(Vector2Int startPos, Vector2Int endPos)
        {
            int minX = Math.Min(startPos.x, endPos.x);
            int maxX = Math.Max(startPos.x, endPos.x);
            int minY = Math.Min(startPos.y, endPos.y);
            int maxY = Math.Max(startPos.y, endPos.y);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector2Int tilePos = new Vector2Int(x, y);
                    DestroyPreviewAt(tilePos);

                    GridTile tile = WorldGrid.Instance.GetTile(tilePos);
                    if (tile.Machine != null)
                    {
                        Destroy(tile.Machine.gameObject);
                    }
                }
            }
        }

        private bool TryGetMouseGridPosition(out Vector2Int gridPosition)
        {
            if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
            {
                gridPosition = new Vector2Int(
                    Mathf.RoundToInt(hit.point.x),
                    Mathf.RoundToInt(hit.point.z)
                );
                return true;
            }

            gridPosition = Vector2Int.zero;
            return false;
        }

        private void SetMainPreviewPosition(Vector2Int gridPosition)
        {
            Vector3 worldPosition = new Vector3(gridPosition.x, GROUND_HEIGHT, gridPosition.y);
            previewContainer.position = worldPosition;
        }

        private void UpdateDestroyRegion(Vector2Int startPos, Vector2Int endPos)
        {
            Vector3 centerPosition = new Vector3(
                (startPos.x + endPos.x) * 0.5f,
                GROUND_HEIGHT,
                (startPos.y + endPos.y) * 0.5f
            );

            Vector3 scale = new Vector3(
                Mathf.Abs(startPos.x - endPos.x) + 1f,
                1f,
                Mathf.Abs(startPos.y - endPos.y) + 1f
            );

            destroyRegion.position = centerPosition;
            destroyRegion.localScale = scale;
            destroyRegion.gameObject.SetActive(true);
        }
    }
}