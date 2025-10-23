using Chipmunk.ComponentContainers;
using Chipmunk.GameEvents;
using Code.SHS.Machines.Events;
using Code.SHS.Machines.ResourceVisualizers;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines.Ports
{
    public class OutputPort : BasePort
    {
        [SerializeField] protected ResourceVisualizers.ResourceVisualizer resourceVisualizer;

        protected IOutputMachine OutputMachine { get; private set; }
        private InputPort linkedInputPort;

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            OutputMachine = Machine as IOutputMachine;
            Debug.Assert(OutputMachine != null, $"can not find IOutputMachine in {Machine.gameObject.name}");
            EventBus<MachineConstructedEvent>.OnEvent += OnMachineConstructed;
            GetLinkedInputPort();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventBus<MachineConstructedEvent>.OnEvent -= OnMachineConstructed;
        }

        private void OnMachineConstructed(MachineConstructedEvent evt)
        {
            GetLinkedInputPort();
        }

        private void GetLinkedInputPort()
        {
            IInputMachine inputMachine =
                WorldGrid.Instance.GetTile(Position + Direction.ToVector2Int()).Machine as IInputMachine;
            linkedInputPort = inputMachine?.GetAvailableInputPort(this);
        }

        public bool CanOutput()
        {
            return Resource == null && linkedInputPort != null && linkedInputPort.CanAcceptResource();
        }

        public virtual bool Output(ShapeResource shapeResource)
        {
            if (CanOutput() == false)
                return false;
            Resource = shapeResource;
            Timer = 0f;
            resourceVisualizer.StartTransport(shapeResource);
            if (resourceVisualizer is TimerResourceVisualizer timerVisualizer)
                timerVisualizer.SetDuration(Interval);
            return true;
        }

        protected override void OnPortTransferComplete()
        {
            if (Resource == null) return;
            Timer = 0f;
            if (linkedInputPort != null)
                SendResourceTo(linkedInputPort);
        }

        protected virtual void SendResourceTo(InputPort inputPort)
        {
            if (inputPort.CanAcceptResource() == false)
                return;

            inputPort.ReceiveResource(Resource);
            Resource = null;
            OutputMachine.OnOutputPortComplete(this);
            resourceVisualizer.EndTransport();
        }
    }
}