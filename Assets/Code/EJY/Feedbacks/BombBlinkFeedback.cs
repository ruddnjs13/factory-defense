using System;
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
        
        private readonly int _blinkHash =  Shader.PropertyToID("_BlinkValue");
        private readonly int _isReadyHash =  Shader.PropertyToID("_IsReady");
        private bool _isStart = false;
        private float _currentBlinkDuration;
        private WaitForSeconds _waitForSeconds;

        private void Awake()
        {
            _waitForSeconds = new WaitForSeconds(blinkDuration);
        }

        public override void CreateFeedback()
        {
            if (!_isStart)
            {
                _currentBlinkDuration = blinkDuration;
                _isStart = true;
                skinRenderer.material.SetInt(_isReadyHash, 1);

                StartCoroutine(BlinkCoroutine());
            }
        }

        private IEnumerator BlinkCoroutine()
        {
            for (int i = 0; i < blinkCnt; i++)
            {
                skinRenderer.material.SetFloat(_blinkHash, blinkIntensity);
                yield return _waitForSeconds;
                skinRenderer.material.SetFloat(_blinkHash, 0);
                yield return _waitForSeconds;
                _currentBlinkDuration *= 0.5f;
            }
            
            _isStart = false;
            skinRenderer.material.SetInt(_isReadyHash, 0);
        }

        public override void StopFeedback()
        {
        }
    }
}