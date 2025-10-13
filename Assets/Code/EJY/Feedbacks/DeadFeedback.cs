using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Feedbacks
{
    public class DeadFeedback : Feedback
    {
        [SerializeField] private Transform targetTrm;
        [SerializeField] private float duration = 0.3f;
        
        public UnityEvent OnFeedbackComplete;
        
        public override void CreateFeedback()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(targetTrm.DOScale(Vector3.zero, duration));
            seq.AppendInterval(duration);
            OnFeedbackComplete?.Invoke();
        }

        public override void StopFeedback()
        {
        }
    }
}