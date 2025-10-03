using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using VHierarchy.Libs;

namespace Blade.Feedbacks
{
    public class DisintegrateFeedback : Feedback
    {
        [SerializeField] private float delayTime = 3f;
        [SerializeField] private VisualEffect feedbackEffect;
        [SerializeField] private SkinnedMeshRenderer targetMeshRenderer;
        
        private bool _isStart = false;
        
        private readonly int _dissolveHeight = Shader.PropertyToID("_DissolveHeight");
        private readonly int _isDissolve = Shader.PropertyToID("_IsDissolve");

        public UnityEvent FeedbackComplete;
        
        public override void CreateFeedback()
        {
            if (_isStart) return;

            _isStart = true;
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delayTime);
            seq.AppendCallback(() => feedbackEffect.Play());
            seq.AppendCallback(() => targetMeshRenderer.material.SetFloat(_isDissolve, 1));
            seq.Append(targetMeshRenderer.material.DOFloat( -2f, _dissolveHeight, 0.8f));
            seq.AppendInterval(2f);
            seq.OnComplete(() => FeedbackComplete?.Invoke());
        }

        public override void StopFeedback()
        {
        }
    }
}