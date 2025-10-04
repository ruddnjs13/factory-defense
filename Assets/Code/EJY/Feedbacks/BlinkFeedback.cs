using DG.Tweening;
using UnityEngine;

namespace Code.Feedbacks
{
    public class BlinkFeedback : Feedback
    {
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private float blinkDuration = 0.15f;
        [SerializeField] private float blinkIntensity = 0.15f;

        private readonly int _blinkHash = Shader.PropertyToID("_BlinkValue");
        
        public override void CreateFeedback() 
        {
            meshRenderer.material.SetFloat(_blinkHash, blinkIntensity);
            DOVirtual.DelayedCall(blinkDuration, StopFeedback);
        }

        public override void StopFeedback()
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.SetFloat(_blinkHash, 0);
            }
        }
    }
}