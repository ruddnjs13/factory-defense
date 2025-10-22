using System.Collections;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    public class ConveyorBelt : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        [SerializeField] private InputPort[] inputPorts;
        [SerializeField] private OutputPort[] outputPorts;

        public ShapeResource Resource { get; private set; }

        public InputPort GetAvailableInputPort(OutputPort outputPort)
        {
            foreach (InputPort inputPort in inputPorts)
            {
                if (inputPort.CanAcceptInputFrom(outputPort))
                    return inputPort;
            }

            return null;
        }

        public bool CanAcceptResource()
        {
            return Resource == null;
        }

        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            TryOutput();
        }

        private void TryOutput()
        {
            if (Resource != null)
                foreach (OutputPort outputPort in outputPorts)
                {
                    if (outputPort.Output(Resource))
                        break;
                }
        }

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            Resource = inputPort.Pop();
            TryOutput();
        }

        public void OnOutputPortComplete(OutputPort port)
        {
            Resource = null;
        }
    }
}