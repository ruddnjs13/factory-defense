using UnityEngine;

namespace Code.SHS.Machines.Ports
{
    public class TwoWayDirectionInputPort : DirectionInputPort
    {
        public override bool CanAcceptInputFrom(OutputPort outputPort)
        {
            Vector2Int directionVec = Position - outputPort.Position;
            Direction direction = directionVec.ToDirection().Opposite();
            if (direction == this.worldDirection || direction == this.worldDirection.Opposite())
                return true;
            return false;
        }
    }
}