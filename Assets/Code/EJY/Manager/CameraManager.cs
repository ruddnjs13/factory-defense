using Code.Events;
using Core.GameEvent;
using Unity.Cinemachine;
using UnityEngine;

namespace Blade.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private GameEventChannelSO cameraEventChannel;

        private void Awake()
        {
            cameraEventChannel.AddListener<ImpulseEvent>(HandleCameraImpulse);
        }

        private void OnDestroy()
        {
            cameraEventChannel.RemoveListener<ImpulseEvent>(HandleCameraImpulse);
        }

        private void HandleCameraImpulse(ImpulseEvent evt)
        {
            if (impulseSource == null) return;
            impulseSource.GenerateImpulse(evt.impulsePower);
        }
    }
}