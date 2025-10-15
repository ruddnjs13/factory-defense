using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;

namespace Code.SHS.Machines
{
    public interface IInputMachine : IMachine
    {
        public InputPort GetAvailableInputPort(OutputPort outputPort);
        public bool CanAcceptResource();
        public void InputPortResourceTransferComplete(InputPort inputPort);
    }
}