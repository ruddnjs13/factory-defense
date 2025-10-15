using Code.SHS.Extensions;
using Code.SHS.Machines.Events;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    public class Miner : BaseMachine, IOutputMachine, IHasResource
    {
        [SerializeField] private ShapeResourceSO testShapeResource;
        [SerializeField] private float mineInterval = 2f;
        [SerializeField] private OutputPort outputPort;
        private float mineTimer;

        public ShapeResource Resource { get; private set; }

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
            Resource = ShapeResource.Create(testShapeResource);
            outputPort.Output(Resource);
        }
    }
}