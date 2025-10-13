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
            StartCoroutine(TransforCoroutine());
        }

        public IEnumerator TransforCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (currentResource != null)
                {
                    ExtractResource();
                }
            }
        }
    }
}