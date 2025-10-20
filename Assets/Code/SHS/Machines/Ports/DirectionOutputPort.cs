using Chipmunk.ComponentContainers;
using Code.SHS.Extensions;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.Worlds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines.Ports
{
    public class DirectionOutputPort : OutputPort
    {
        [SerializeField] private Direction direction;
        protected Direction worldDirection;

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);

            float yRotation = transform.eulerAngles.y;
            worldDirection = direction.Rotate(yRotation);
        }


        protected override InputPort FindInputPort()
        {
            Vector2Int outputPosition =
                Position + Vector3Int.RoundToInt(worldDirection.ToVector3()).ToXZ();
            InputPort inputPort = InputPort(outputPosition);
            if (inputPort != null && inputPort.CanAcceptInputFrom(this) && inputPort.CanAcceptResource())
                return inputPort;
            return null;
        }

        protected virtual InputPort InputPort(Vector2Int outputPosition)
        {
            WorldTile outputTile = WorldGrid.Instance.GetTile(outputPosition);
            BaseMachine machine = outputTile.Machine;
            if (machine == null || machine is not IInputMachine inputMachine) return null;
            return inputMachine.GetAvailableInputPort(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            float yRotation = transform.eulerAngles.y;
            worldDirection = direction.Rotate(yRotation);
            Vector3 directionVector = worldDirection.ToVector3();
            Vector3 position = transform.position + directionVector + new Vector3(0f, 0.5f, 0f);
            Gizmos.DrawWireCube(position, Vector3.one * 1f);
        }
    }
}