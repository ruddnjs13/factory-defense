using System.Collections.Generic;
using Chipmunk.GameEvents;
using Chipmunk.Player;
using Chipmunk.UI;
using Code.SHS.Extensions;
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
        public Dictionary<Vector2Int, ConstructPreview> PreviewByPosition { get; } = new();

        private bool isLeftClicking;
        private bool isRemoveMode;
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
            previewContainer.rotation = Quaternion.identity;
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
            {
                Destroy(mainPreview.gameObject);
            }

            MachineSO machine = evt.MachineSo;
            mainPreview = Instantiate(machine.machinePreviewPrefab, previewContainer.position, machine.rotation,
                    previewContainer)
                .GetComponent<ConstructPreview>();

            Debug.Assert(mainPreview != null, "ConstructPreview component not found on machinePreviewPrefab");

            Vector2Int startGridPosition = previewContainer.position.ToInt().ToXZ();
            mainPreview.Initialize(machine, this, startGridPosition);
            previewContainer.gameObject.SetActive(true);
        }

        private void OnLeftClick(bool isPressed)
        {
            isLeftClicking = isPressed;
            if (UIPointerDetector.IsPointerInUI)
                return;

            if (isPressed)
            {
                PressLeftClick();
            }
            else
            {
                ReleaseLeftClick();
            }
        }

        private void PressLeftClick()
        {
            if (playerInput.ShiftKeyPressed && TryGetMouseGridPosition(out Vector2Int position))
            {
                currentGridPosition = position;
                isRemoveMode = true;
            }
        }

        private void ReleaseLeftClick()
        {
            if (isRemoveMode)
            {
                ExecuteRemoveMode();
            }
            else if (mainPreview != null)
            {
                {
                    mainPreview.gameObject.SetActive(true);
                    if (PreviewByPosition.TryGetValue(currentGridPosition, out ConstructPreview preview) == false)
                        AddPreviewAtPosition(currentGridPosition, Direction.None);
                }
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


            mainPreview.UpdatePosition(newPosition);
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

            if (PreviewByPosition.TryGetValue(fromPosition, out ConstructPreview existingPreview))
            {
                if (existingPreview is SelectorConstructPreview conveyorPreview)
                {
                    conveyorPreview.AddOutputDirection(worldDirection);
                }
            }
            else
            {
                mainPreview.gameObject.SetActive(true);
                AddPreviewAtPosition(fromPosition, worldDirection);
            }

            if (PreviewByPosition.TryGetValue(toPosition, out existingPreview) &&
                existingPreview is SelectorConstructPreview toConveyorPreview)
            {
                mainPreview.gameObject.SetActive(false);
                toConveyorPreview.AddInputDirection(worldDirection.Opposite());
            }
        }

        private void AddPreviewAtPosition(Vector2Int gridPosition, Direction nextDirection)
        {
            if (!CanPlaceMachineAtPosition(gridPosition))
                return;

            RemoveOverlappingPreviews(gridPosition);
            MachineSO machineSO = mainPreview.MachineSO;

            mainPreview.transform.SetParent(null);
            ConstructPreview preview = mainPreview;
            mainPreview = Instantiate(preview, preview.transform.position, preview.transform.rotation,
                previewContainer);
            mainPreview.name = preview.name;
            mainPreview.Initialize(machineSO, this, gridPosition);

            if (mainPreview is SelectorConstructPreview mainSelectorPreview)
            {
                mainSelectorPreview.AddInputDirection(nextDirection.Opposite());
            }

            if (preview is SelectorConstructPreview selectorPreview)
            {
                selectorPreview.AddOutputDirection(nextDirection);
            }

            preview.gameObject.SetActive(true);
            previewInstances.Add(preview);
            RegisterPreviewAtPosition(gridPosition, preview);
        }

        private void RemoveOverlappingPreviews(Vector2Int gridPosition)
        {
            IterateTiles(gridPosition, DestroyPreviewAt);
        }

        private void RegisterPreviewAtPosition(Vector2Int gridPosition, ConstructPreview preview)
        {
            IterateTiles(gridPosition, tilePos => PreviewByPosition[tilePos] = preview);
        }

        private bool CanPlaceMachineAtPosition(Vector2Int gridPosition)
        {
            bool canPlace = true;
            IterateTiles(gridPosition, tilePos =>
            {
                GridTile tile = WorldGrid.Instance.GetTile(tilePos);
                if (tile.Machine != null)
                {
                    canPlace = false;
                }
            });

            return canPlace;
        }

        private void IterateTiles(Vector2Int gridPosition, System.Action<Vector2Int> action)
        {
            Vector2Int size = mainPreview.MachineSO.size;
            Vector2Int offset = mainPreview.MachineSO.offset;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int tilePos = gridPosition + new Vector2Int(x, y) + offset;
                    action?.Invoke(tilePos);
                }
            }
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

            CancelConstruction();
        }

        private void ExecuteRemoveMode()
        {
            if (!TryGetMouseGridPosition(out Vector2Int position))
                return;

            IterateTiles(position, DestroyPreviewAt);
            isRemoveMode = false;
            mainPreview.gameObject.SetActive(false);
        }

        private void DestroyPreviewAt(Vector2Int gridPosition)
        {
            if (PreviewByPosition.TryGetValue(gridPosition, out ConstructPreview preview))
            {
                preview.gameObject.SetActive(false);
                PreviewByPosition.Remove(gridPosition);
            }
        }


        private bool TryGetMouseGridPosition(out Vector2Int gridPosition)
        {
            gridPosition = default;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
            {
                gridPosition = hit.point.ToInt().ToXZ();
                return true;
            }

            return false;
        }

        private void CancelConstruction()
        {
            if (mainPreview != null)
            {
                Destroy(mainPreview.gameObject);
                mainPreview = null;
            }

            isRemoveMode = false;

            foreach (ConstructPreview preview in previewInstances)
            {
                if (preview != null)
                {
                    Destroy(preview.gameObject);
                }
            }

            previewInstances.Clear();
            PreviewByPosition.Clear();
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

        private void OnDrawGizmos()
        {
            if (mainPreview != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(mainPreview.transform.position, Vector3.one);
            }
        }
    }
}