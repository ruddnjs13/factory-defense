using Code.SHS.Extensions;
using Code.SHS.Machines.ResourceVisualizer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines.Ports
{
    public class TwoWayDirectionOutputPort : DirectionOutputPort
    {

        protected override InputPort FindInputPort()
        {
            Vector2Int outputPosition =
                Position + Direction.GetDirection(worldDirection.Opposite()).ToInt().ToXZ();
            InputPort inputPort = InputPort(outputPosition);
            if (inputPort != null && inputPort.CanAcceptInputFrom(this) && inputPort.CanAcceptResource())
                return inputPort;
            return base.FindInputPort();
        }

        public override void SendResourceTo(InputPort inputPort)
        {
            base.SendResourceTo(inputPort);
            // inverseResourceVisualizer.EndTransport();

            DirectionEnum direction = Direction.GetDirection(inputPort.Position - Position);
            // if (direction == worldDirection.Opposite())
                // inverseResourceVisualizer.EndTransportReverse();
            // else
                // resourceVisualizer.EndTransport();
        }
    }
}