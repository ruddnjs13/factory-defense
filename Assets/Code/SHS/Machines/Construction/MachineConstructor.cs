using System;
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
        private GameObject machinePreview;
        private bool isLeftClicking = false;

        private void Awake()
        {
            EventBus<MachineSelectEvent>.OnEvent += OnMachineSelect;
            playerInput.OnLeftClickEvent += LeftClickHandler;
            playerInput.OnRightClickEvent += RightClickHandler;
            playerInput.OnRotateEvent += RotateHandler;
        }

        private void OnDestroy()
        {
            EventBus<MachineSelectEvent>.OnEvent -= OnMachineSelect;
            playerInput.OnLeftClickEvent -= LeftClickHandler;
            playerInput.OnRightClickEvent -= RightClickHandler;
            playerInput.OnRotateEvent -= RotateHandler;
        }

        private void OnMachineSelect(MachineSelectEvent evt)
        {
            machineSO = evt.MachineSo;
            if (machinePreview != null)
            {
                Destroy(machinePreview);
            }

            machinePreview = Instantiate(machineSO.machineGhostPrefab);
        }

        private void Update()
        {
            if (machinePreview != null)
            {
                if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
                {
                    Vector3 position = Vector3Int.RoundToInt(hit.point) * new Vector3Int(1, 0, 1);
                    machinePreview.transform.position = position;
                    if (isLeftClicking && WorldGrid.Instance.GetTile(position).Machine == null)
                        Instantiate(machineSO.machinePrefab, position, machinePreview.transform.rotation);
                }
            }
        }

        private void LeftClickHandler(bool isPressed)
        {
            isLeftClicking = isPressed;
            if (isLeftClicking == false)
            {
                StopConstruction();
            }
            // 기존 일회성 클릭 로직
            // if (isPressed && machineSO != null)
            // {
            // if (playerInput.MousePositionRaycast(out RaycastHit hit, layerMask))
            // {
            //     // Vector3 position = Vector3Int.RoundToInt(hit.point) * new Vector3Int(1, 0, 1);
            //     Vector3 position = machinePreview.transform.position;
            //     Instantiate(machineSO.machinePrefab, position, Quaternion.identity);
            // }
            // }
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
            if (machinePreview != null)
            {
                machinePreview.transform.Rotate(Vector3.up, 90f);
            }
        }

        private void CancelConstruction()
        {
            machineSO = null;
            if (machinePreview != null)
            {
                Destroy(machinePreview);
            }
        }

        private void StopConstruction()
        {
            machineSO = null;
            if (machinePreview != null)
            {
                Destroy(machinePreview);
            }
        }
    }
}