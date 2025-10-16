using System;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.ResourceVisualizer;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines.Ports
{
    public abstract class InputPort : BasePort
    {
        [SerializeField] private BaseResourceVisualizer resourceVisualizer;

        protected IInputMachine InputMachine { get; private set; }

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            InputMachine = Machine as IInputMachine;
            Debug.Assert(InputMachine != null, $"can not find IInputMachine in {Machine.gameObject.name}");
        }

        public abstract bool CanAcceptInputFrom(OutputPort outputPort);

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