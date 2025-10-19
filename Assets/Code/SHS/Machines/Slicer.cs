using Chipmunk.ComponentContainers;
using Code.SHS.Animations;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ResourceVisualizer;
using Code.SHS.Machines.ShapeResources;
using Code.Units.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Code.SHS.Machines
{
    [RequireComponent(typeof(Animator), typeof(ParameterAnimator), typeof(AnimatorTrigger))]
    public class Slicer : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        private static readonly bool[] SlicePieces = { false, false, true, true, false, false, true, true };
        [SerializeField] private ParameterSO workParam;
        [SerializeField] private InputPort inputPort;
        [SerializeField] private BaseResourceVisualizer resourceVisualizer;
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
            resourceVisualizer.StartTransport(Resource);
        }

        public void OnOutputComplete(OutputPort port)
        {
        }

        private void HandleAnimationTrigger()
        {
            if (Resource == null) return;
            for (int i = 0; i < 8; i++)
            {
                // SlicePieces 배열 정보를 바탕으로 잘라서 남길 조각은 남기고, 버릴 조각은 null로 설정
                Resource.ShapePieces[i].ShapePieceSo = SlicePieces[i] ? Resource.ShapePieces[i].ShapePieceSo : null;
            }

            if (outputPort.Output(Resource))
            {
                resourceVisualizer.EndTransport();
                Resource = null;
            }
        }
    }
}