using UnityEngine;

namespace Code.SHS.Machines.Ports
{
    public class CompositeInputPort : MonoBehaviour
    {
        [SerializeField] private InputPort[] ports;
        public InputPort GetAvailablePort(OutputPort port)
        {
            foreach (InputPort inputPort in ports)
            {
                if (inputPort.CanAcceptInputFrom(port))
                    return inputPort;
            }
            return null;
        }
    }
}