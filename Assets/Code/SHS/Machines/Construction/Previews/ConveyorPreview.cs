using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class ConveyorPreview : ConstructPreview
    {
        private ConveyorSO conveyorSO;

        [SerializeField] private MeshFilter meshFilter;

        public override void Initialize(MachineSO machineSO, MachineConstructor constructor)
        {
            base.Initialize(machineSO, constructor);
            conveyorSO = (ConveyorSO)machineSO;
            Mesh mesh = meshFilter.mesh;
            foreach (ConveyorData conveyorData in conveyorSO.conveyorDataList)
            {
            }
        }

        public bool isValidData(ConveyorData data)
        {
            foreach (Vector2Int localPosition in data.InputPositions)
            {
                Vector2Int worldPosition = localPosition + Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z));
                // ConstructPreview constructor.previewByPosition.
            }

            return true;
        }
    }
}