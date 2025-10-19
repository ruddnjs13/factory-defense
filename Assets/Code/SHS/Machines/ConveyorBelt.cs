using System.Collections;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines
{
    public class ConveyorBelt : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        [SerializeField] private InputPort inputPort;
        [SerializeField] private OutputPort outputPort;

        public ShapeResource Resource { get; private set; }

        public InputPort GetAvailableInputPort(OutputPort outputPort) =>
            inputPort.CanAcceptInputFrom(outputPort) ? inputPort : null;

        public bool CanAcceptResource()
        {
            return Resource == null;
        }

        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            if (Resource != null)
                outputPort.Output(Resource);
        }

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            Resource = inputPort.Pop();
            outputPort.Output(Resource);
        }

        public void OnOutputComplete(OutputPort port)
        {
            Resource = null;
        }
    }
}