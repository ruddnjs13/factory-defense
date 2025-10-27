using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class ConveyorPreview : ConstructPreview
    {
        private ConveyorSO conveyorSO;

        [SerializeField] private MeshFilter meshFilter;
        private HashSet<Direction> outputDirections = new HashSet<Direction>();
        private HashSet<Direction> inputDirections = new HashSet<Direction>();
        private ConveyorData conveyorData;

        public override void Initialize(MachineSO machineSO, MachineConstructor constructor)
        {
            base.Initialize(machineSO, constructor);
            conveyorSO = (ConveyorSO)machineSO;
            outputDirections.Clear();
            inputDirections.Clear();
        }

        public void AddInputDirection(Direction direction)
        {
            float transformY = transform.rotation.eulerAngles.y;
            float baseY = conveyorSO.rotation.eulerAngles.y;
            direction = direction.Rotate(baseY - transformY);

            outputDirections.Remove(direction);
            inputDirections.Add(direction);
            UpdateConveyorPreview();
        }

        public void AddOutputDirection(Direction direction)
        {
            float transformY = transform.rotation.eulerAngles.y;
            float baseY = conveyorSO.rotation.eulerAngles.y;
            direction = direction.Rotate(baseY - transformY);

            if (direction == Direction.Back)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
                inputDirections.Remove(direction);
                outputDirections.Remove(direction);
                UpdateConveyorPreview();
            }
            else
            {
                inputDirections.Remove(direction);
                outputDirections.Add(direction);
                UpdateConveyorPreview();
            }
        }

        private void UpdateConveyorPreview()
        {
            Mesh mesh = meshFilter.mesh;
            foreach (ConveyorData data in conveyorSO.conveyorDataList)
            {
                if (isValidData(data))
                {
                    mesh = data.mesh;
                    this.conveyorData = data;
                }
            }

            meshFilter.sharedMesh = mesh;
        }

        public bool isValidData(ConveyorData data)
        {
            foreach (Direction direction in data.OutputDirections)
            {
                if (outputDirections.Contains(direction) == false)
                {
                    return false;
                }
            }

            foreach (Direction direction in data.InputDirections)
            {
                if (inputDirections.Contains(direction) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override BaseMachine CreateInstance()
        {
            if (conveyorData != null)
            {
                Debug.Assert(conveyorData.prefab != null, "Conveyor prefab is not assigned in ConveyorData.");
                BaseMachine machine = Instantiate(conveyorData.prefab, transform.position, transform.rotation)
                    .GetComponent<BaseMachine>();
                return machine;
            }

            return base.CreateInstance();
        }
    }
}