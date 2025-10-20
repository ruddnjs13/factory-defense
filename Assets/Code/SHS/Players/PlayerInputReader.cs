using System;
using Chipmunk.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Chipmunk.Player
{
    [CreateAssetMenu(fileName = "PlayerInputReader", menuName = "SO/PlayerInputReader", order = 0)]
    public class PlayerInputReader : ScriptableObject, Controls.IPlayerActions
    {
        public Action<bool> OnLeftClickEvent;
        public Action<bool> OnRightClickEvent;
        public Action<bool> OnMiddleClickEvent;
        public Action<int> OnNumberKeyEvent;
        public Action<float> OnWheelEvent;
        public Action OnRotateEvent;
        public Vector2 MousePosition { get; private set; }
        public Vector2 MoveDirection { get; private set; }
        public bool ShiftKeyPressed { get; private set; }
        private Controls controls;

        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new Controls();
                controls.Player.SetCallbacks(this);
            }

            controls.Enable();
            controls.Player.Enable();
        }

        private void OnDisable()
        {
            if (controls != null)
            {
                controls.Player.RemoveCallbacks(this);
                controls.Player.Disable();
                controls.Disable();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if (context.performed && UIPointerDetector.IsPointerInUI == false)
                OnLeftClickEvent?.Invoke(true);
            else if (context.canceled)
                OnLeftClickEvent?.Invoke(false);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (context.performed && UIPointerDetector.IsPointerInUI == false)
                OnRightClickEvent?.Invoke(true);
            else if (context.canceled)
                OnRightClickEvent?.Invoke(false);
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            if (context.performed && UIPointerDetector.IsPointerInUI == false)
                OnMiddleClickEvent?.Invoke(true);
            else if (context.canceled)
                OnMiddleClickEvent?.Invoke(false);
        }

        public void OnMouse(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnNumber(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                int numberPressed = (int)context.ReadValue<float>();
                OnNumberKeyEvent?.Invoke(numberPressed);
            }
        }

        public void OnWheel(InputAction.CallbackContext context)
        {
            if (context.performed && UIPointerDetector.IsPointerInUI == false)
            {
                var scroll = context.ReadValue<Vector2>().y;
                OnWheelEvent?.Invoke(scroll);
            }
        }

        public void OnShift(InputAction.CallbackContext context)
        {
            if (context.performed)
                ShiftKeyPressed = true;
            else if (context.canceled)
                ShiftKeyPressed = false;
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnRotateEvent?.Invoke();
        }

        public bool MousePositionRaycast(out RaycastHit hit, LayerMask targetLayerMask)
        {
            hit = default;
            Camera cam = Camera.main;
            if (cam == null) return false;
            Ray ray = cam.ScreenPointToRay(MousePosition);
            if (Physics.Raycast(ray, out hit, cam.farClipPlane, targetLayerMask))
                return true;
            return false;
        }
        // public Ray GetMouseRay()
        // {
        //     Ph
        // }
    }
}