using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class ConveyorPreview : ConstructPreview
    {
        private ConveyorSO conveyorSO;

        [SerializeField] private MeshFilter meshFilter;
        private HashSet<Direction> directions = new HashSet<Direction>();
        private ConveyorData conveyorData;

        public override void Initialize(MachineSO machineSO, MachineConstructor constructor)
        {
            base.Initialize(machineSO, constructor);
            conveyorSO = (ConveyorSO)machineSO;
        }

        public override void SetNextDirection(Direction nextDirection)
        {
            base.SetNextDirection(nextDirection);
            directions.Add(nextDirection);
            Mesh mesh = meshFilter.mesh;
            Debug.Log(nextDirection);
            foreach (ConveyorData conveyorData in conveyorSO.conveyorDataList)
            {
                if (isValidData(conveyorData))
                {
                    mesh = conveyorData.mesh;
                    this.conveyorData = conveyorData;
                }
            }

            meshFilter.sharedMesh = mesh;
        }

        public bool isValidData(ConveyorData data)
        {
            foreach (Direction direction in data.OutputDirections)
            {
                if (directions.Contains(direction) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override GameObject Construct()
        {
            if (conveyorData != null)
            {
                GameObject machine = Instantiate(conveyorData.prefab, transform.position, transform.rotation);
                return machine;
            }

            return base.Construct();
        }
    }
}