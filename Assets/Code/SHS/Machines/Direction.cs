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
            new Vector3(0, 0, 1), // Forward (앞, Z+)
            new Vector3(1, 0, 0), // Right (오른쪽, X+)
            new Vector3(0, 0, -1), // Back (뒤, Z-)
            new Vector3(-1, 0, 0), // Left (왼쪽, X-)
        };

        public static Vector2Int[] directionsInt = new Vector2Int[4]
        {
            new Vector2Int(0, 1), // Forward (앞)
            new Vector2Int(1, 0), // Right (오른쪽)
            new Vector2Int(0, -1), // Back (뒤)
            new Vector2Int(-1, 0), // Left (왼쪽)
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
        /// Y축 회전 각도를 기반으로 방향을 시계 방향으로 회전시킵니다.
        /// </summary>
        /// <param name="direction">원본 방향</param>
        /// <param name="yRotation">Y축 회전 각도 (임의의 각도)</param>
        /// <returns>회전된 방향</returns>
        public static DirectionEnum RotateDirection(DirectionEnum direction, float yRotation)
        {
            if (direction == DirectionEnum.None) return DirectionEnum.None;

            // Y축 회전을 90도 단위로 정규화 (시계 방향: Forward -> Right -> Back -> Left)
            int rotationSteps = Mathf.RoundToInt(yRotation / 90f) % 4;
            if (rotationSteps < 0) rotationSteps += 4; // 음수 회전을 양수로 변환

            // 방향을 회전 (시계 방향)
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

        public static Quaternion GetQuaternionFromDirection(DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Forward:
                    return Quaternion.Euler(0f, 0f, 0f);
                case DirectionEnum.Right:
                    return Quaternion.Euler(0f, 90f, 0f);
                case DirectionEnum.Back:
                    return Quaternion.Euler(0f, 180f, 0f);
                case DirectionEnum.Left:
                    return Quaternion.Euler(0f, 270f, 0f);
                default:
                    return Quaternion.identity;
            }
        }
    }


    public enum DirectionEnum
    {
        None = -1,
        Forward = 0, // 앞 (Z+, 0도)
        Right = 1, // 오른쪽 (X+, 90도)
        Back = 2, // 뒤 (Z-, 180도)
        Left = 3, // 왼쪽 (X-, 270도)
    }

    public static class DirectionExtensions
    {
        public static DirectionEnum Opposite(this DirectionEnum direction) => Direction.GetOpposite(direction);
    }
}