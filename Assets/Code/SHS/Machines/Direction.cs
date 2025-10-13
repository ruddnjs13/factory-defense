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
            new Vector3(0, 0, 1),   // Forward (앞, Z+)
            new Vector3(1, 0, 0),   // Right (오른쪽, X+)
            new Vector3(0, 0, -1),  // Back (뒤, Z-)
            new Vector3(-1, 0, 0),  // Left (왼쪽, X-)
        };

        public static Vector2Int[] directionsInt = new Vector2Int[4]
        {
            new Vector2Int(0, 1),   // Forward (앞)
            new Vector2Int(1, 0),   // Right (오른쪽)
            new Vector2Int(0, -1),  // Back (뒤)
            new Vector2Int(-1, 0),  // Left (왼쪽)
        };

        public static int[] directionX = new int[4] { 0, 1, 0, -1 };
        public static int[] directionY = new int[4] { 1, 0, -1, 0 };

        public static Vector3 GetDirection(DirectionEnum dir)
            => directions[(int)dir];
        public static Vector2Int GetTileDirection(DirectionEnum dir)
            => directionsInt[(int)dir];
        public static DirectionEnum GetDirection(Vector2Int direction)
        {
            DirectionEnum dir = DirectionEnum.Forward;
            for (int i = 0; i < 4; i++)
            {
                if (directionsInt[i] == direction) dir = (DirectionEnum)i;
            }

            return dir;
        }

        public static DirectionEnum GetOpposite(DirectionEnum dir) => (DirectionEnum)(((int)dir + 2) % 4);

        /// <summary>
        /// Y축 회전 각도를 기반으로 방향을 회전시킵니다.
        /// </summary>
        /// <param name="direction">원본 방향</param>
        /// <param name="yRotation">Y축 회전 각도 (0, 90, 180, 270)</param>
        /// <returns>회전된 방향</returns>
        public static DirectionEnum RotateDirection(DirectionEnum direction, float yRotation)
        {
            if (direction == DirectionEnum.None) return DirectionEnum.None;
            
            // Y축 회전을 90도 단위로 정규화 (0, 90, 180, 270)
            int rotationSteps = Mathf.RoundToInt(yRotation / 90f) % 4;
            if (rotationSteps < 0) rotationSteps += 4;
            
            // 방향을 회전
            int newDirection = ((int)direction + rotationSteps) % 4;
            return (DirectionEnum)newDirection;
        }

        public static DirectionEnum GetBoundaryExitDirection(Vector2Int min, Vector2Int max, Vector2Int np)
        {
            DirectionEnum direction = DirectionEnum.Forward;

            if (np.x < min.x) direction = DirectionEnum.Left;
            if (np.x > max.x) direction = DirectionEnum.Right;
            if (np.y < min.y) direction = DirectionEnum.Back;
            if (np.y > max.y) direction = DirectionEnum.Forward;

            return direction;
        }
    }


    public enum DirectionEnum
    {
        None = -1,
        Forward = 0,  // 앞 (Z+, 0도)
        Right = 1,    // 오른쪽 (X+, 90도)
        Back = 2,     // 뒤 (Z-, 180도)
        Left = 3,     // 왼쪽 (X-, 270도)
    }
}
