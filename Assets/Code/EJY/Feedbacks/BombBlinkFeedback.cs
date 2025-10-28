using System.Collections;
using UnityEngine;

namespace Code.Feedbacks
{
    public class BombBlinkFeedback : Feedback
    {
        [SerializeField] private int blinkCnt = 5;
        [SerializeField] private float blinkIntensity = 0.6f;
        [SerializeField] private float blinkDuration = 0.3f;
        [SerializeField] private SkinnedMeshRenderer skinRenderer;
        
        private readonly int _blinkHash = Shader.PropertyToID("_BlinkValue");
        private readonly int _isReadyHash = Shader.PropertyToID("_IsReady");

        private bool _isStart = false;
        private float _currentBlinkDuration;
        private Coroutine _blinkCoroutine;

        public override void CreateFeedback()
        {
            if (_isStart) return;

            _currentBlinkDuration = blinkDuration;
            _isStart = true;

            skinRenderer.material.SetInt(_isReadyHash, 1);

            _blinkCoroutine = StartCoroutine(BlinkCoroutine());
        }

        private IEnumerator BlinkCoroutine()
        {
            for (int i = 0; i < blinkCnt; i++)
            {
                skinRenderer.material.SetFloat(_blinkHash, blinkIntensity);
                yield return new WaitForSeconds(_currentBlinkDuration);
                skinRenderer.material.SetFloat(_blinkHash, 0);
                yield return new WaitForSeconds(_currentBlinkDuration);
                
                _currentBlinkDuration *= 0.5f;
            }

            skinRenderer.material.SetInt(_isReadyHash, 0);
            _isStart = false;
            _blinkCoroutine = null;
        }

        public override void StopFeedback()
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
                _blinkCoroutine = null;
            }

            skinRenderer.material.SetFloat(_blinkHash, 0);
            skinRenderer.material.SetInt(_isReadyHash, 0);
            _isStart = false;
        }
    }
}
