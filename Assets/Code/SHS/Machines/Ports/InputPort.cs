using System;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.ResourceVisualizers;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines.Ports
{
    public class InputPort : BasePort
    {
        [SerializeField] private ResourceVisualizers.ResourceVisualizer resourceVisualizer;

        protected IInputMachine InputMachine { get; private set; }

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            InputMachine = Machine as IInputMachine;
        }

        public virtual bool CanAcceptInputFrom(OutputPort outputPort)
        {
            // Debug.Log("Checking input port acceptance");
            // Debug.Log("Output port position: " + outputPort.Position);
            // Debug.Log("Input port position + direction: " + (Position + Direction.ToVector2Int()));
            return outputPort.Position == (Position + Direction.ToVector2Int());
        }

        public virtual bool CanAcceptResource()
        {
            return Resource == null && InputMachine.CanAcceptResource();
        }

        public virtual void ReceiveResource(ShapeResource shapeResource)
        {
            Timer = 0f;
            Resource = shapeResource;
            resourceVisualizer.StartTransport(shapeResource);
            if (resourceVisualizer is TimerResourceVisualizer timerVisualizer)
                timerVisualizer.SetDuration(Interval);
        }

        protected override void OnPortTransferComplete()
        {
            InputMachine.InputPortResourceTransferComplete(this);
        }

        public ShapeResource Pop()
        {
            ShapeResource res = Resource;
            Resource = null;
            resourceVisualizer.EndTransport();
            return res;
        }
    }
}