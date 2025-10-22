using Chipmunk.ComponentContainers;
using Code.SHS.Animations;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using Code.Units.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    [RequireComponent(typeof(Animator), typeof(ParameterAnimator), typeof(AnimatorTrigger))]
    public class Stacker : BaseMachine, IInputMachine, IOutputMachine
    {
        // [SerializeField] private CompositeInputPort inputPort;
        //이건 나중에 수정할거임
        [SerializeField] private InputPort leftInputPort;
        [SerializeField] private InputPort rightInputPort;
        [SerializeField] private OutputPort outputPort;
        [SerializeField] private ParameterSO activeParameter;
        [SerializeField] private ParameterAnimator parameterAnimator;
        [SerializeField] private AnimatorTrigger animatorTrigger;

        private bool isProcessing = false;

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            parameterAnimator = this.Get<ParameterAnimator>();
            animatorTrigger = this.Get<AnimatorTrigger>();

            animatorTrigger.OnAnimationTrigger += HandleAnimationTrigger;
        }


        public InputPort GetAvailableInputPort(OutputPort outputPort)
        {
            if (leftInputPort.CanAcceptInputFrom(outputPort))
                return leftInputPort;
            if (rightInputPort.CanAcceptInputFrom(outputPort))
                return rightInputPort;
            return null;
        }

        public bool CanAcceptResource()
            => outputPort.CanOutput();

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            if (leftInputPort.Resource != null && rightInputPort.Resource != null && !isProcessing)
            {
                isProcessing = true;
                parameterAnimator.SetParameter(activeParameter);
            }
        }

        private void HandleAnimationTrigger()
        {
            ShapeResource leftResource = leftInputPort.Pop();
            ShapeResource rightResource = rightInputPort.Pop();

            ShapeResource stackedResource = ShapeResource.Stack(leftResource, rightResource);

            outputPort.Output(stackedResource);
            isProcessing = false;
        }

        public void OnOutputPortComplete(OutputPort port)
        {
        }
    }
}