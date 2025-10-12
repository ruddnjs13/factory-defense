using DG.Tweening;
using UnityEngine;

namespace Code.Feedbacks
{
    public class DeadFeedback : Feedback
    {
        [SerializeField] private Transform targetTrm;
        [SerializeField] private float duration = 0.3f;
        private Tween _deathTween;
        
        public override void CreateFeedback()
        {
            _deathTween = targetTrm.DOScale(Vector3.zero, duration);
        }

        public override void StopFeedback()
        {
        }
    }
}