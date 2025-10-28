using System;
using System.Collections.Generic;
using System.Linq;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class SelectorConstructPreview : ConstructPreview
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private SelectConstructSO selectConstructSO;

        public readonly HashSet<PortData> availableWorldInputPorts = new HashSet<PortData>();
        public readonly HashSet<PortData> availableWorldOutputPorts = new HashSet<PortData>();

        public override void Initialize(MachineSO machineSO, MachineConstructor machineConstructor, Vector2Int position)
        {
            base.Initialize(machineSO, machineConstructor, position);
            // transform.rotation = machineSO.rotation;
            //
            // // 4방향 모두 시도하여 최적의 방향 찾기
            // if (TryFindValidRotation(out Direction validRotation))
            // {
            //     // 찾은 방향으로 회전 설정
            //     transform.rotation = validRotation.ToQuaternion();
            //
            //     // 해당 회전으로 연결 분석 및 머신 선택
            //     AnalyzeConnections();
            //     SelectMachine();
            // }
            // else
            // {
            //     Debug.LogWarning("No valid rotation found for SelectorConstructPreview at " + position, this);
            // }
        }

        public void ResetSelector()
        {
            availableWorldInputPorts.Clear();
            availableWorldOutputPorts.Clear();
            AnalyzeConnections();
            SelectMachine();
        }

        private bool TryFindValidRotation(out Direction validRotation)
        {
            Direction[] directions = { Direction.Forward, Direction.Right, Direction.Back, Direction.Left };

            Quaternion originalRotation = transform.rotation;
            foreach (Direction testDirection in directions)
            {
                Debug.Log(testDirection);
                transform.rotation = testDirection.ToQuaternion();

                availableWorldInputPorts.Clear();
                availableWorldOutputPorts.Clear();
                AnalyzeConnections();

                foreach (MachineSO candidateMachine in selectConstructSO.machineSOList)
                {
                    if (IsMatchingMachine(candidateMachine, transform.rotation.eulerAngles.y))
                    {
                        validRotation = testDirection;
                        Debug.Log(
                            $"Found valid rotation: {testDirection} ({testDirection.ToQuaternion().eulerAngles.y}°)",
                            this);
                        return true;
                    }
                }
            }

            transform.rotation = originalRotation;
            validRotation = Direction.Forward;
            return false;
        }

        private void AnalyzeConnections()
        {
            CheckPortConnections(selectConstructSO.inputPorts, availableWorldInputPorts, isInputPort: true);
            CheckPortConnections(selectConstructSO.outputPorts, availableWorldOutputPorts, isInputPort: false);

            string portInfo = "";
            foreach (PortData port in availableWorldOutputPorts)
            {
                portInfo += "I:" + port.FacingDirection.Rotate(transform.rotation.eulerAngles.y) + " ";
                portInfo += '\n';
            }

            Debug.Log(availableWorldInputPorts.Count + " input ports activated.", this);
            Debug.Log(portInfo + availableWorldOutputPorts.Count + " output ports activated.", this);
        }

        private void CheckPortConnections(List<PortData> portsToCheck, HashSet<PortData> activatedPorts,
            bool isInputPort)
        {
            // Debug.Log(portsToCheck.Count + " ports checking.", this);
            foreach (PortData portData in portsToCheck)
            {
                float myRotationY = transform.rotation.eulerAngles.y;
                Direction worldFacingDirection = portData.FacingDirection.Rotate(myRotationY);
                // Debug.Log(worldFacingDirection + " " + myRotationY);
                Vector2Int targetPosition = portData.TargetPosition() + Position;

                if (IsConnectedToPreview(targetPosition, worldFacingDirection, isInputPort))
                {
                    activatedPorts.Add(portData);
                }
                else if (IsConnectedToPlacedMachine(portData, isInputPort))
                {
                    Debug.Log(
                        "Connected to placed machine at " + targetPosition + portData.FacingDirection +
                        (isInputPort ? "input" : "output"), this);
                    activatedPorts.Add(portData);
                }
            }
        }

        private bool IsConnectedToPreview(Vector2Int targetPosition, Direction myWorldDirection, bool isInputPort)
        {
            if (!constructor.PreviewByPosition.TryGetValue(targetPosition, out ConstructPreview neighborPreview))
                return false;

            var neighborPorts =
                isInputPort ? neighborPreview.MachineSO.outputPorts : neighborPreview.MachineSO.inputPorts;
            Direction requiredDirection = myWorldDirection.Opposite();

            // 상대방 프리뷰의 회전도 고려
            float neighborRotationY = neighborPreview.transform.rotation.eulerAngles.y;
            return neighborPorts.Any(port =>
                port.FacingDirection.Rotate(neighborRotationY) == requiredDirection);
        }

        private bool IsConnectedToPlacedMachine(PortData targetPortData, bool isInputPort)
        {
            float myRotationY = transform.rotation.eulerAngles.y;
            Direction worldPortDirection = targetPortData.FacingDirection.Rotate(myRotationY);
            // Debug.Log("Checking connection to placed machine at " +
            //           (targetPortData.TargetPosition(myRotationY) + Position) + " for " +
            //           (isInputPort ? "input" : "output") + " port.", this);
            Vector2Int targetPosition = targetPortData.LocalPosition + worldPortDirection.ToVector2Int() + Position;
            var tile = WorldGrid.Instance.GetTile(targetPosition);
            BaseMachine targetMachine = tile.Machine;

            if (targetMachine is null)
                return false;

            bool isValidType = isInputPort ? targetMachine is IOutputMachine : targetMachine is IInputMachine;
            if (!isValidType)
                return false;

            var neighborPorts = isInputPort ? targetMachine.MachineSo.outputPorts : targetMachine.MachineSo.inputPorts;
            Direction requiredDirection = worldPortDirection.Opposite();

            float targetRotationY = targetMachine.transform.rotation.eulerAngles.y;
            foreach (PortData neighborPort in neighborPorts)
            {
                Vector2Int neighborPosition = neighborPort.LocalPosition + targetMachine.Position;
                if (neighborPosition == targetPosition
                    && neighborPort.FacingDirection.Rotate(targetRotationY) == requiredDirection)
                    return true;
            }

            return false;
        }

        public void SelectMachine()
        {
            foreach (MachineSO candidateMachine in selectConstructSO.machineSOList)
            {
                if (IsMatchingMachine(candidateMachine, transform.rotation.eulerAngles.y))
                {
                    float prevRotationY = MachineSO.rotation.eulerAngles.y;
                    MachineSO = candidateMachine;
                    float deltaRotationY =
                        candidateMachine.rotation.eulerAngles.y - prevRotationY;
                    transform.Rotate(0, deltaRotationY, 0);

                    var meshFilterComponent =
                        candidateMachine.machinePrefab?.GetComponentInChildren<MeshFilter>();
                    Debug.Assert(meshFilterComponent != null, "meshFilterComponent is null");
                    if (meshFilterComponent != null)
                    {
                        meshFilter.mesh = meshFilterComponent.sharedMesh;
                    }

                    return;
                }
            }
        }

        private bool IsMatchingMachine(MachineSO candidateMachine, float Rotation)
        {
            // float adjustedRotation = candidateMachine.rotation.eulerAngles.y;
            float adjustedRotation = candidateMachine.rotation.eulerAngles.y +
                                     (transform.rotation.eulerAngles.y - MachineSO.rotation.eulerAngles.y);
            // Debug.Log($"Checking machine {candidateMachine.name} at rotation {adjustedRotation}", this);
            if (candidateMachine.inputPorts.Count > 1)
            {
                foreach (var port in candidateMachine.inputPorts)
                {
                    // Debug.Log($"{port.FacingDirection} : {port.GetRotatedPortData(adjustedRotation).FacingDirection}");
                    if (!availableWorldInputPorts.Contains(
                            port.GetRotatedPortData(adjustedRotation)))
                    {
                        return false;
                    }
                }
            }

            foreach (var port in candidateMachine.outputPorts)
            {
                // Debug.Log(port.GetRotatedPortData(adjustedRotation).FacingDirection);
                if (!availableWorldOutputPorts.Contains(
                        port.GetRotatedPortData(adjustedRotation)))
                    return false;
            }

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            // Debug.Log("OnDrawGizmosSelected");
            foreach (PortData portData in availableWorldOutputPorts)
            {
                Vector3 localPos = new Vector3(portData.LocalPosition.x, 0.1f, portData.LocalPosition.y);
                Vector3 worldPos = transform.TransformPoint(localPos);
                Vector2Int directionVector2Int = portData.FacingDirection.ToVector2Int();
                Vector3 directionVector = new Vector3(directionVector2Int.x, 0,
                    directionVector2Int.y);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(worldPos, directionVector);
                // Debug.Log("output " + portData.FacingDirection);
            }

            foreach (PortData portData in availableWorldInputPorts)
            {
                Vector3 localPos = new Vector3(portData.LocalPosition.x, 0.1f, portData.LocalPosition.y);
                Vector3 worldPos = transform.TransformPoint(localPos);
                Vector2Int directionVector2Int = portData.FacingDirection.ToVector2Int();
                Vector3 directionVector = new Vector3(directionVector2Int.x, 0,
                    directionVector2Int.y);
                Gizmos.color = Color.green;
                Gizmos.DrawRay(worldPos, directionVector);
                // Debug.Log("input " + portData.FacingDirection);
            }
        }

        public void AddOutputDirection(Direction nextDirection)
        {
            if (availableWorldOutputPorts.Count == 0)
            {
                Direction currentFancing =
                    Direction.Forward.Rotate(transform.rotation.eulerAngles.y - MachineSO.rotation.eulerAngles.y);
                if (nextDirection.Opposite() == currentFancing)
                    transform.Rotate(0, 180, 0);
            }

            availableWorldOutputPorts.Add(new PortData
            {
                FacingDirection = nextDirection
            });
            SelectMachine();
        }

        public void AddInputDirection(Direction nextDirection)
        {
            if (availableWorldInputPorts.Count == 0)
            {
                transform.rotation = nextDirection.ToQuaternion();
            }

            availableWorldInputPorts.Add(new PortData
            {
                FacingDirection = nextDirection
            });
            SelectMachine();
        }
    }
}