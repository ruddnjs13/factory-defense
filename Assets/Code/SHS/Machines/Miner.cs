using Chipmunk.ComponentContainers;
using Code.SHS.Extensions;
using Code.SHS.Machines.Events;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.Worlds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    public class Miner : BaseMachine, IOutputMachine, IHasResource
    {
        [SerializeField] private ShapeResourceSO mineShapeResource;
        [SerializeField] private float mineInterval = 2f;
        [SerializeField] private OutputPort outputPort;
        private float mineTimer;

        public ShapeResource Resource { get; private set; }

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
        }

        protected override void Construct()
        {
            base.Construct();
            if (WorldGrid.Instance.GetTile(Position).Ground is ResourceGroundSO resourceGround)
                mineShapeResource = resourceGround.ResourceSO;
            else
            {
                Debug.LogError("Miner must be placed on ResourceGround");
                Destroy(gameObject);
            }
        }

        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            if (mineTimer >= mineInterval)
                MineResource();
            mineTimer += deltaTime;
        }

        public void OnOutputComplete(OutputPort port)
        {
            Resource = null;
        }

        protected override void MachineConstructHandler(MachineConstructEvent evt)
        {
            base.MachineConstructHandler(evt);
        }

        private void MineResource()
        {
            if (Resource != null) return;
            mineTimer -= mineInterval;
            Resource = ShapeResource.Create(mineShapeResource);
            outputPort.Output(Resource);
        }
    }
}