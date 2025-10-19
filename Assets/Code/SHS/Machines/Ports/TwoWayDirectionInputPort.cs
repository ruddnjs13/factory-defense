using UnityEngine;

namespace Code.SHS.Machines.Ports
{
    public class TwoWayDirectionInputPort : DirectionInputPort
    {
        public override bool CanAcceptInputFrom(OutputPort outputPort)
        {
            Vector2Int directionVec = Position - outputPort.Position;
            DirectionEnum direction = Direction.GetOpposite(Direction.GetDirection(directionVec));
            if (direction == this.worldDirection || direction == Direction.GetOpposite(this.worldDirection))
                return true;
            return false;
        }
    }
}