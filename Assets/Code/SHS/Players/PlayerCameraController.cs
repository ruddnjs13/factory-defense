using System;
using Chipmunk.ComponentContainers;
using Chipmunk.StatSystem;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using EventBus = Unity.VisualScripting.EventBus;

namespace Chipmunk.Player
{
    public class PlayerCameraController : MonoBehaviour, IContainerComponent
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private Vector2 yClamp = new Vector2(-90f, 90f);
        [SerializeField] private StatSO rotationSpeedStat;
        [SerializeField] private float defaultOrthographicSize = 20f;
        [SerializeField] private float focusOrthographicSize = 10f;
        [SerializeField] private float sizeTransitionSpeed = 4f;
        [SerializeField] private Vector2 minMaxZoom = new Vector2(-8f, 10);
        private float targetOrthographicSize = 20f;
        private float currentZoom = 0f;
        private bool isRotating;
        public ComponentContainer ComponentContainer { get; set; }
        private Vector2 previousMousePosition;

        public void OnInitialize(ComponentContainer componentContainer)
        {
            inputReader.OnRightClickEvent += RightClickHandler;
            inputReader.OnWheelEvent += ScrollHandler;

            targetOrthographicSize = defaultOrthographicSize;
        }

        private void OnDestroy()
        {
            inputReader.OnRightClickEvent -= RightClickHandler;
            inputReader.OnWheelEvent -= ScrollHandler;
        }


        private void Update()
        {
            if (isRotating)
            {
                Vector2 currentMousePosition = inputReader.MousePosition;
                Vector2 delta = currentMousePosition - previousMousePosition;

                transform.Rotate(Vector3.up, delta.x * rotationSpeedStat.Value, Space.World);
                float currentX = transform.localEulerAngles.x;
                float newX = Mathf.Clamp(currentX + delta.y * rotationSpeedStat.Value, yClamp.x, yClamp.y);
                transform.localEulerAngles =
                    new Vector3(newX, transform.localEulerAngles.y, transform.localEulerAngles.z);

                previousMousePosition = currentMousePosition;
            }

            // currentZoom = Mathf.Clamp(currentZoom, targetOrthographicSize + 1, minMaxZoom.y);
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,
                (targetOrthographicSize + currentZoom), Time.deltaTime * sizeTransitionSpeed);
        }


        private void ScrollHandler(float obj)
        {
            currentZoom = Mathf.Clamp(currentZoom - obj, minMaxZoom.x, minMaxZoom.y);
        }

        private void RightClickHandler(bool isPressed)
        {
            isRotating = isPressed;
            previousMousePosition = inputReader.MousePosition;
        }
    }
}