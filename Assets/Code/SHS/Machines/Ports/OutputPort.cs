using Chipmunk.ComponentContainers;
using Code.SHS.Machines.ResourceVisualizer;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines.Ports
{
    public abstract class OutputPort : BasePort
    {
        [SerializeField] protected BaseResourceVisualizer resourceVisualizer;

        protected IOutputMachine OutputMachine { get; private set; }

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            OutputMachine = Machine as IOutputMachine;
            Debug.Assert(OutputMachine != null, $"can not find IOutputMachine in {Machine.gameObject.name}");
        }

        public bool CanOutput()
        {
            return Resource == null;
        }

        public virtual bool Output(ShapeResource shapeResource)
        {
            if (CanOutput() == false)
                return false;
            Resource = shapeResource;
            Timer = 0f;
            resourceVisualizer.StartTransport(shapeResource, Interval);
            return true;
        }

        protected override void OnPortTransferComplete()
        {
            if (Resource == null) return;
            Timer = 0f;
            InputPort port = FindInputPort();
            if (port != null)
                SendResourceTo(port);
        }

        protected abstract InputPort FindInputPort();

        public virtual void SendResourceTo(InputPort inputPort)
        {
            if (inputPort.CanAcceptInputFrom(this) == false || inputPort.CanAcceptResource() == false)
                return;

            inputPort.ReceiveResource(Resource);
            Resource = null;
            OutputMachine.OnOutputComplete(this);
            resourceVisualizer.EndTransport();
        }
    }
}