using Chipmunk.ComponentContainers;
using Code.SHS.Animations;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using Code.Units.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Code.SHS.Machines
{
    public class Slicer : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        private static readonly bool[] SlicePieces = { true, true, false, false, true, true, false, false };
        [SerializeField] private ParameterSO workParam;
        [SerializeField] private InputPort inputPort;
        [SerializeField] private OutputPort outputPort;
        private ParameterAnimator parameterAnimator;
        private AnimatorTrigger animatorTrigger;

        public ShapeResource Resource { get; private set; }

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            parameterAnimator = this.Get<ParameterAnimator>();
            animatorTrigger = this.Get<AnimatorTrigger>();
            animatorTrigger.OnAnimationTrigger += HandleAnimationTrigger;
        }


        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            // if (Resource != null && outputPort.Output(Resource))
                // Resource = null;
        }

        public InputPort GetAvailableInputPort(OutputPort outputPort) =>
            inputPort.CanAcceptInputFrom(outputPort) ? inputPort : null;

        public bool CanAcceptResource()
        {
            return Resource == null && outputPort.CanOutput();
        }

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            Resource = inputPort.Pop();
            parameterAnimator.SetParameter(workParam);
        }

        public void OnOutputComplete(OutputPort port)
        {
        }

        private void HandleAnimationTrigger()
        {
            if (Resource == null) return;
            for (int i = 0; i < 8; i++)
            {
                Resource.ResourcePieces[i] = SlicePieces[i] ? Resource.ResourcePieces[i] : null;
            }

            if (outputPort.Output(Resource))
                Resource = null;
        }
    }
}