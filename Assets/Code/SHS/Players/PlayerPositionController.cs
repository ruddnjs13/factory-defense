using System;
using UnityEngine;

namespace Chipmunk.Player
{
    public class PlayerPositionController : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private float moveSpeed = 5f;

        private void Update()
        {
            Vector3 move = inputReader.MoveDirection;
            move = Camera.main.transform.TransformDirection(move);
            move.y = 0f;
            float speed = moveSpeed;
            if (inputReader.ShiftKeyPressed)
                speed *= 2f;
            transform.Translate(move.normalized * (speed * Time.deltaTime), Space.World);
        }
    }
}