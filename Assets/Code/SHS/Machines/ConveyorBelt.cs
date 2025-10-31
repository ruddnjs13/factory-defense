using System.Collections;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ResourceVisualizers;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    [RequireComponent(typeof(MaterialInstancer))]
    public class ConveyorBelt : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        [SerializeField] private InputPort[] inputPorts;
        [SerializeField] private OutputPort[] outputPorts;

        [SerializeField] private ResourceVisualizer resourceVisualizer;
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
            {
                foreach (OutputPort outputPort in outputPorts)
                {
                    if (outputPort.Output(Resource))
                    {
                        Resource = null;
                        resourceVisualizer.EndTransport();
                        break;
                    }
                }
            }
        }

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            if (Resource == null)
            {
                Resource = inputPort.Pop();
                resourceVisualizer.StartTransport(Resource);
            }

            TryOutput();
        }

        public void OnOutputPortComplete(OutputPort port)
        {
        }
    }
}