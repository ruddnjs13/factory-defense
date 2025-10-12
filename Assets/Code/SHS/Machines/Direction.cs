using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines
{
    public static class Direction
    {
        public static Vector3[] directions = new Vector3[4]
        {
            new Vector3(0, 0, 1),   // Back (앞)
            new Vector3(1, 0, 0),   // Right (오른쪽)
            new Vector3(0, 0, -1),  // Front (뒤)
            new Vector3(-1, 0, 0),  // Left (왼쪽)
        };

        public static Vector2Int[] directionsInt = new Vector2Int[4]
        {
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        };

        public static int[] directionX = new int[4] { 0, 0, 1, -1 };
        public static int[] directionY = new int[4] { 1, -1, 0, 0 };

        public static Vector3 GetDirection(DirectionEnum dir)
            => directions[(int)dir];
        public static Vector2Int GetTileDirection(DirectionEnum dir)
            => directionsInt[(int)dir];
        public static DirectionEnum GetDirection(Vector2Int direction)
        {
            DirectionEnum dir = DirectionEnum.Back;
            for (int i = 0; i < 4; i++)
            {
                if (directionsInt[i] == direction) dir = (DirectionEnum)i;
            }

            return dir;
        }

        public static DirectionEnum GetOpposite(DirectionEnum dir) => (DirectionEnum)(((int)dir + 2) % 4);

        public static DirectionEnum GetBoundaryExitDirection(Vector2Int min, Vector2Int max, Vector2Int np)
        {
            DirectionEnum direction = DirectionEnum.Back;

            if (np.x < min.x) direction = DirectionEnum.Left;
            if (np.x > max.x) direction = DirectionEnum.Right;
            if (np.y < min.y) direction = DirectionEnum.Back;
            if (np.y > max.y) direction = DirectionEnum.Front;

            return direction;
        }
    }


    public enum DirectionEnum
    {
        None = -1,
        Back = 0,
        Right = 1,
        Front = 2,
        Left = 3,
    }
}
