using System;
using System.Collections.Generic;
using Code.SHS.Machines;
using Code.SHS.Machines.Ports;
using UnityEngine;

namespace Code.SHS.Machines
{
    [CreateAssetMenu(fileName = "MachineSO", menuName = "Machine/MachineSO", order = 0)]
    public class MachineSO : ScriptableObject
    {
        public string machineName;
        [field: SerializeField] public Sprite Icon { get; private set; }
        public GameObject machinePreviewPrefab;
        public GameObject machinePrefab;
        public Vector2Int size = Vector2Int.one;
        public Vector2Int offset = Vector2Int.zero;
        public Quaternion rotation;
        public int cost = 20;

        [NonSerialized] public List<PortData> inputPorts = new List<PortData>();

        [NonSerialized] public List<PortData> outputPorts = new List<PortData>();

        private void OnEnable()
        {
            SetupPortData();
        }

        private void SetupPortData()
        {
            if (machinePrefab == null)
                return;
            inputPorts.Clear();
            outputPorts.Clear();
            foreach (BasePort port in machinePrefab.GetComponentsInChildren<BasePort>())
            {
                List<PortData> targetPortList = null;
                if (port is InputPort)
                    targetPortList = inputPorts;
                else if (port is OutputPort)
                    targetPortList = outputPorts;

                if (targetPortList == null) continue;

                targetPortList.Add(new PortData()
                {
                    LocalPosition = new Vector2Int(Mathf.RoundToInt(port.transform.localPosition.x),
                        Mathf.RoundToInt(port.transform.localPosition.z)),
                    FacingDirection = port.LocalDirection
                });
            }
        }
    }
}

public struct PortData : IEquatable<PortData>
{
    public Vector2Int LocalPosition;
    public Direction FacingDirection;

    public Vector2Int TargetPosition(float rotation = 0)
    {
        Vector2 rotatedPosition = Quaternion.Euler(0, 0, rotation) * new Vector2(LocalPosition.x, LocalPosition.y);
        Direction facingDirection = FacingDirection.Rotate(rotation);
        return new Vector2Int(Mathf.RoundToInt(rotatedPosition.x), Mathf.RoundToInt(rotatedPosition.y)) +
               facingDirection.ToVector2Int();
    }

    public PortData GetRotatedPortData(float rotation)
    {
        Vector2 rotatedPosition = Quaternion.Euler(0, 0, rotation) * new Vector2(LocalPosition.x, LocalPosition.y);
        Direction facingDirection = FacingDirection.Rotate(rotation);
        return new PortData()
        {
            LocalPosition = new Vector2Int(Mathf.RoundToInt(rotatedPosition.x), Mathf.RoundToInt(rotatedPosition.y)),
            FacingDirection = facingDirection
        };
    }

    public bool Equals(PortData other)
    {
        return LocalPosition == other.LocalPosition && FacingDirection == other.FacingDirection;
    }

    public override bool Equals(object obj)
    {
        return obj is PortData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LocalPosition, FacingDirection);
    }
}