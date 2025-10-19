using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines
{
    public class Splitter : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        [SerializeField] private InputPort inputPort;
        [SerializeField] private OutputPort outputPort;

        public ShapeResource Resource { get; private set; }

        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            if (Resource != null)
                outputPort.Output(Resource);
        }

        public InputPort GetAvailableInputPort(OutputPort outputPort) =>
            inputPort.CanAcceptInputFrom(outputPort) ? inputPort : null;

        public bool CanAcceptResource()
        {
            return Resource == null;
        }

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            Resource = inputPort.Pop();
            outputPort.Output(Resource);
        }

        public void OnOutputComplete(OutputPort port)
        {
        }
    }
}