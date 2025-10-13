using Code.SHS.Extensions;
using Code.SHS.Machines.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    public class Miner : ResourceTransporter
    {
        [SerializeField] private ResourceSO testResource;

        [SerializeField] private float mineInterval = 2f;
        private float mineTimer;

        public override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            if (mineTimer >= mineInterval)
                MineResource();
            mineTimer += deltaTime;
        }

        protected override void MachineConstructHandler(MachineConstructEvent evt)
        {
            base.MachineConstructHandler(evt);
        }

        private void MineResource()
        {
            Debug.Log("MineResource");
            mineTimer -= mineInterval;
            if (currentResource != null) return;
            ReceiveResource(new Resource(testResource));
        }
    }
}