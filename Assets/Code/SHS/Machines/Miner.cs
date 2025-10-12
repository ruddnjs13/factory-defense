using Code.SHS.Extensions;
using Code.SHS.Machines.Events;
using UnityEngine;

namespace Code.SHS.Machines
{
    public class Miner : ResourceTransporter
    {
        [SerializeField] private float miningInterval = 2f;
        [SerializeField] private ResourceSO testResource;
        private float miningTimer = 0f;

        public override void Update()
        {
            base.Update();
            miningTimer += Time.deltaTime;

            if (miningTimer >= miningInterval)
            {
                miningTimer = 0f;
                MineResource();
            }
        }

        protected override void MachineConstructHandler(MachineConstructEvent evt)
        {
            base.MachineConstructHandler(evt);
        }

        private void MineResource()
        {
            ExtractResource();
            if (currentResource != null) return;
            currentResource = new Resource(testResource);
            resourceVisual = Instantiate(testResource.prefab, transform);
        }
    }
}