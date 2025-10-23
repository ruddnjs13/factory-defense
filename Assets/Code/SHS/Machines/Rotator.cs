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
    public class Rotator : BaseMachine, IInputMachine, IOutputMachine, IHasResource
    {
        // private static readonly int[] SwapData_ClockWise = { 3, 1, 2, 4, 7, 5, 8, 6 };
        private static readonly int[] RotationData_ClockWise = { 2, 4, 1, 3, 6, 8, 5, 7 };

        private static readonly int[] RotationData_CounterClockWise = { 3, 1, 4, 2, 7, 5, 8, 6 };
        [SerializeField] private bool clockwise = true;
        [SerializeField] private ParameterSO workParam;
        [SerializeField] private ParameterSO rotateParam;
        [FormerlySerializedAs("baseInputPort")] [SerializeField] private InputPort inputPort;
        [SerializeField] private ResourceVisualizers.ResourceVisualizer resourceVisualizer;
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

        public void OnOutputPortComplete(OutputPort port)
        {
        }

        private void HandleAnimationTrigger()
        {
            if (Resource == null) return;
            ShapePiece[] beforePieces = new ShapePiece[8];
            for (int i = 0; i < 8; i++)
            {
                beforePieces[i] = Resource.ShapePieces[i];
                if (Resource.ShapePieces[i].ShapePieceSo != null)
                {
                    beforePieces[i].Rotation *= Quaternion.Euler(0, clockwise ? -90 : 90, 0);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                Resource.ShapePieces[i] =
                    beforePieces[clockwise ? RotationData_ClockWise[i] - 1 : RotationData_CounterClockWise[i] - 1];
            }
            // Debug.Log(Quaternion.identity * Quaternion.Euler(0, clockwise ? -90 : 90, 0));

            if (outputPort.Output(Resource))
            {
                Resource = null;
                
                resourceVisualizer.EndTransport();
            }
        }
    }
}