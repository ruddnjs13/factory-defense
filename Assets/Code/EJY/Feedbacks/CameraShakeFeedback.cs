using System;
using Code.Entities;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Feedbacks
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraShakeFeedback : Feedback
    {
        [SerializeField] private ActionData actionData;
        [SerializeField] private bool onlyPlayPowerAttack = true;
        [SerializeField] private float impulseForce = 0.6f;
        private CinemachineImpulseSource _impulseSource;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public override void CreateFeedback()
        {
            if(onlyPlayPowerAttack == false || actionData.HitByPowerAttack)
                _impulseSource.GenerateImpulse(impulseForce);
        }

        public override void StopFeedback()
        {
            // do noting
        }
    }
}