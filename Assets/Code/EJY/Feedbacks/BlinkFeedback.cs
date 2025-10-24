using System;
using DG.Tweening;
using UnityEngine;

namespace Code.Feedbacks
{
    public class BlinkFeedback : Feedback
    {
        [SerializeField] private Transform targetVisualTrm;
        [SerializeField] private float blinkDuration = 0.15f;
        [SerializeField] private float blinkIntensity = 0.15f;

        private readonly int _blinkHash = Shader.PropertyToID("_BlinkValue");
        private readonly int _isReadyHash =  Shader.PropertyToID("_IsReady");
        private SkinnedMeshRenderer[] _meshRenderers;

        private void Awake()
        {
             _meshRenderers = targetVisualTrm.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        public override void CreateFeedback() 
        {
            if(_isReadyHash == 1)
                return;
            
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.material.SetFloat(_blinkHash, blinkIntensity);
            }
            DOVirtual.DelayedCall(blinkDuration, StopFeedback).SetLink(transform.root.gameObject);
        }

        public override void StopFeedback()
        {
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.material.SetFloat(_blinkHash, 0);
            }
        }
    }
}