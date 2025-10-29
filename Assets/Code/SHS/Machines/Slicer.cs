using Chipmunk.ComponentContainers;
using Code.SHS.Animations;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ResourceVisualizers;
using Code.SHS.Machines.ShapeResources;
using Code.Units.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    [RequireComponent(typeof(Animator), typeof(ParameterAnimator), typeof(AnimatorTrigger))]
    public class Slicer : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        private static readonly bool[] SlicePieces = { false, false, true, true, false, false, true, true };
        [SerializeField] private ParameterSO workParam;

        [FormerlySerializedAs("baseInputPort")] [SerializeField]
        private InputPort inputPort;

        [SerializeField] private ResourceVisualizer resourceVisualizer;
        [SerializeField] private OutputPort outputPort;
        private ParameterAnimator parameterAnimator;
        private AnimatorTrigger animatorTrigger;
        
        public ShapeResource Resource { get; private set; }

        private bool isProcessing = false;
        
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
            if (!isProcessing && Resource != null && outputPort.Output(Resource))
            {
                resourceVisualizer.EndTransport();
                Resource = null;
            }
        }

        public InputPort GetAvailableInputPort(OutputPort fromPort) =>
            inputPort.CanAcceptInputFrom(fromPort) ? inputPort : null;

        public bool CanAcceptResource()
        {
            return Resource == null;
        }

        public void InputPortResourceTransferComplete(InputPort fromPort)
        {
            Resource = fromPort.Pop();
            // 리소스가 들어오면 작업(커팅)을 시작하도록 플래그 설정
            isProcessing = true;
            parameterAnimator.SetParameter(workParam);
            resourceVisualizer.StartTransport(Resource);
        }

        public void OnOutputPortComplete(OutputPort port)
        {
        }

        private void HandleAnimationTrigger()
        {
            if (Resource == null) return;

            for (int i = 0; i < 8; i++)
            {
                Resource.ShapePieces[i].ShapePieceSo = SlicePieces[i] ? Resource.ShapePieces[i].ShapePieceSo : null;
            }

            isProcessing = false;

            if (outputPort.Output(Resource))
            {
                resourceVisualizer.EndTransport();
                Resource = null;
            }
            else
            {
                resourceVisualizer.StartTransport(Resource);
            }
        }
    }
}