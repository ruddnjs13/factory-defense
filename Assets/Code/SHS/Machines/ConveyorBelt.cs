using System.Collections;
using Chipmunk.ComponentContainers;
using UnityEngine;

namespace Code.SHS.Machines
{
    public class ConveyorBelt : ResourceTransporter
    {
        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);
        }

        public override bool TryReceiveResource(IOutputResource machine, Resource resource)
        {
            return base.TryReceiveResource(machine, resource);
        }
    }
}