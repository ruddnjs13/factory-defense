using Chipmunk.ComponentContainers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines.Ports
{
    public class DirectionInputPort : InputPort
    {
        [FormerlySerializedAs("inputDirection")] [SerializeField]
        private DirectionEnum direction;

        protected DirectionEnum worldDirection;

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);

            float yRotation = transform.eulerAngles.y;
            worldDirection = Direction.RotateDirection(direction, yRotation);
        }

        public override bool CanAcceptInputFrom(OutputPort outputPort)
        {
            Vector2Int directionVec = Position - outputPort.Position;
            DirectionEnum direction = Direction.GetOpposite(Direction.GetDirection(directionVec));
            if (direction != this.worldDirection)
                return false;
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            float yRotation = transform.eulerAngles.y;
            worldDirection = Direction.RotateDirection(direction, yRotation);
            Vector3 directionVector = Direction.GetDirection(worldDirection);
            Vector3 position = transform.position + directionVector + new Vector3(0f, 0.5f, 0f);
            Gizmos.DrawWireCube(position, Vector3.one * 1f);
        }
    }
}