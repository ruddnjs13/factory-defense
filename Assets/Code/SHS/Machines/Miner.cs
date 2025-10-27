using Chipmunk.ComponentContainers;
using Code.SHS.Animations;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ResourceVisualizers;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.Worlds;
using Code.Units.Animations;
using UnityEngine;

namespace Code.SHS.Machines
{
    /// <summary>
    /// 자원 지형에서 자원을 채굴하는 기계
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(ParameterAnimator), typeof(AnimatorTrigger))]
    public class Miner : BaseMachine, IOutputMachine, IHasResource
    {
        [SerializeField] private ParameterSO activateParam;
        [SerializeField] private ParameterSO closeParam;
        [SerializeField] private OutputPort outputPort;
        [SerializeField] private AnimatableResourceVisualizer resourceVisualizer;

        private ShapeResourceSO mineShapeResource;
        private ParameterAnimator parameterAnimator;
        private AnimatorTrigger animatorTrigger;
        private bool canOutput;

        public ShapeResource Resource { get; private set; }

        #region Unity Lifecycle & Initialization

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
            parameterAnimator = this.Get<ParameterAnimator>();
            animatorTrigger = this.Get<AnimatorTrigger>();
            animatorTrigger.OnAnimationTrigger += OnMineResource;
            animatorTrigger.OnAnimationEnd += OnMineComplete;

            if (WorldGrid.Instance.GetTile(Position).Ground is ResourceGroundSO resourceGround)
            {
                mineShapeResource = resourceGround.ResourceSO;
            }
            else
            {
                Debug.LogError("Miner must be placed on ResourceGround");
                Destroy(gameObject);
            }
        }

        #endregion

        #region Tick & Logic

        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);

            // 자원이 없으면 채굴 애니메이션 시작
            if (Resource == null)
            {
                parameterAnimator.SetParameter(activateParam);
                return;
            }

            // 출력 가능 상태이고 자원이 있으면 출력 시도
            if (canOutput)
            {
                TryOutputResource();
            }
        }

        #endregion

        #region Output Interface

        public void OnOutputPortComplete(OutputPort port)
        {
            Resource = null;
            canOutput = false;
            parameterAnimator.SetParameter(closeParam);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 자원 출력 시도
        /// </summary>
        private void TryOutputResource()
        {
            if (!outputPort.CanOutput() || Resource == null) return;

            if (outputPort.Output(Resource))
            {
                canOutput = false;
                resourceVisualizer.EndTransport();
            }
        }

        /// <summary>
        /// 애니메이션 트리거 시점에 자원 생성
        /// </summary>
        private void OnMineResource()
        {
            Resource = ShapeResource.Create(mineShapeResource);
            resourceVisualizer.StartTransport(Resource);
        }

        /// <summary>
        /// 채굴 애니메이션 종료 시 출력 가능 상태로 전환
        /// </summary>
        private void OnMineComplete()
        {
            canOutput = true;
            TryOutputResource();
        }

        #endregion
    }
}