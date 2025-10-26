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
        [FormerlySerializedAs("leftBaseInputPort")] [SerializeField] private InputPort leftInputPort;
        [FormerlySerializedAs("rightBaseInputPort")] [SerializeField] private InputPort rightInputPort;
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
            => true;
            // => outputPort.CanOutput();

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

            // We used the left/right resources to form the stacked resource; return them to the pool
            if (leftResource != null)
                leftResource.Release();
            if (rightResource != null)
                rightResource.Release();
        }

        public void OnOutputPortComplete(OutputPort port)
        {
        }
    }
}