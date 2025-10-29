using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines
{
    /// <summary>
    /// 4방향을 나타내는 열거형 (앞, 오른쪽, 뒤, 왼쪽)
    /// </summary>
    public enum Direction
    {
        /// <summary>방향 없음</summary>
        None = -1,

        /// <summary>앞 (Z+, 0도)</summary>
        Forward = 0,

        /// <summary>오른쪽 (X+, 90도)</summary>
        Right = 1,

        /// <summary>뒤 (Z-, 180도)</summary>
        Back = 2,

        /// <summary>왼쪽 (X-, 270도)</summary>
        Left = 3,
    }

    /// <summary>
    /// Direction enum을 위한 확장 메서드 모음
    /// </summary>
    public static class DirectionExtensions
    {
        // 방향 배열들
        private static readonly Vector3[] _directions = new Vector3[4]
        {
            new Vector3(0, 0, 1), // Forward (앞, Z+)
            new Vector3(1, 0, 0), // Right (오른쪽, X+)
            new Vector3(0, 0, -1), // Back (뒤, Z-)
            new Vector3(-1, 0, 0), // Left (왼쪽, X-)
        };

        private static readonly Vector2Int[] _directionsInt = new Vector2Int[4]
        {
            new Vector2Int(0, 1), // Forward (앞)
            new Vector2Int(1, 0), // Right (오른쪽)
            new Vector2Int(0, -1), // Back (뒤)
            new Vector2Int(-1, 0), // Left (왼쪽)
        };

        private static readonly int[] _directionX = new int[4] { 0, 1, 0, -1 };
        private static readonly int[] _directionY = new int[4] { 1, 0, -1, 0 };

        /// <summary>
        /// Direction을 3D 방향 벡터로 변환합니다.
        /// </summary>
        /// <param name="dir">변환할 방향</param>
        /// <returns>방향에 해당하는 Vector3</returns>
        public static Vector3 ToVector3(this Direction dir)
        {
            if (dir == Direction.None) return Vector3.zero;
            return _directions[(int)dir];
        }

        /// <summary>
        /// Direction을 2D 타일 방향 벡터로 변환합니다.
        /// </summary>
        /// <param name="dir">변환할 방향</param>
        /// <returns>방향에 해당하는 Vector2Int</returns>
        public static Vector2Int ToVector2Int(this Direction dir)
        {
            if (dir == Direction.None) return Vector2Int.zero;
            return _directionsInt[(int)dir];
        }

        /// <summary>
        ///  3D 방향 벡터를 Direction으로 변환합니다.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Direction ToDirection(this Vector3 dir)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_directions[i] == dir) return (Direction)i;
            }

            return Direction.None;
        }

        /// <summary>
        ///  2D 타일 방향 벡터를 Direction으로 변환합니다.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Direction ToDirection(this Vector2Int dir)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_directionsInt[i] == dir) return (Direction)i;
            }

            return Direction.None;
        }

        /// <summary>
        /// Direction을 Y축 회전 Quaternion으로 변환합니다.
        /// </summary>
        /// <param name="direction">변환할 방향</param>
        /// <returns>방향에 해당하는 Quaternion</returns>
        public static Quaternion ToQuaternion(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Forward:
                    return Quaternion.Euler(0f, 0f, 0f);
                case Direction.Right:
                    return Quaternion.Euler(0f, 90f, 0f);
                case Direction.Back:
                    return Quaternion.Euler(0f, 180f, 0f);
                case Direction.Left:
                    return Quaternion.Euler(0f, 270f, 0f);
                default:
                    return Quaternion.identity;
            }
        }

        /// <summary>
        /// 반대 방향을 반환합니다.
        /// </summary>
        /// <param name="direction">기준 방향</param>
        /// <returns>반대 방향</returns>
        public static Direction Opposite(this Direction direction) =>
            direction == Direction.None ? direction : (Direction)(((int)direction + 2) % 4);

        /// <summary>
        /// Y축 회전값을 기준으로 방향을 시계방향으로 회전시킵니다.
        /// </summary>
        /// <param name="direction">기준 방향</param>
        /// <param name="yRotation">Y축 회전 각도</param>
        /// <returns>회전된 방향</returns>
        public static Direction Rotate(this Direction direction, float yRotation)
        {
            if (direction == Direction.None) return Direction.None;

            int rotationSteps = Mathf.RoundToInt(yRotation / 90f) % 4;
            if (rotationSteps < 0) rotationSteps += 4;

            int newDirection = ((int)direction + rotationSteps) % 4;
            return (Direction)newDirection;
        }

        /// <summary>
        /// 월드 좌표계 Direction을 특정 Quaternion의 로컬 좌표계로 변환합니다.
        /// </summary>
        /// <param name="worldDirection">월드 좌표계 방향</param>
        /// <param name="localRotation">로컬 좌표계의 회전값</param>
        /// <returns>로컬 좌표계로 변환된 방향</returns>
        public static Direction ToLocalDirection(this Direction worldDirection, Quaternion localRotation)
        {
            if (worldDirection == Direction.None) return Direction.None;

            // 월드 방향 벡터를 로컬 좌표계로 변환
            Vector3 worldVector = worldDirection.ToVector3();
            Vector3 localVector = Quaternion.Inverse(localRotation) * worldVector;

            // 가장 가까운 방향 찾기
            float maxDot = -1f;
            Direction closestDirection = Direction.Forward;

            for (int i = 0; i < 4; i++)
            {
                Direction dir = (Direction)i;
                float dot = Vector3.Dot(localVector.normalized, dir.ToVector3());
                if (dot > maxDot)
                {
                    maxDot = dot;
                    closestDirection = dir;
                }
            }

            return closestDirection;
        }

        /// <summary>
        /// 타일 이동 벡터를 Direction으로 변환합니다.
        /// </summary>
        /// <param name="direction">타일 이동 벡터</param>
        /// <returns>벡터에 해당하는 Direction</returns>
        public static Direction FromTileDirection(Vector2Int direction)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_directionsInt[i] == direction) return (Direction)i;
            }

            return Direction.Forward;
        }

        /// <summary>
        /// 경계를 벗어나는 방향을 계산합니다.
        /// </summary>
        /// <param name="min">최소 경계</param>
        /// <param name="max">최대 경계</param>
        /// <param name="np">검사할 위치</param>
        /// <returns>경계를 벗어나는 방향</returns>
        public static Direction GetBoundaryExitDirection(Vector2Int min, Vector2Int max, Vector2Int np)
        {
            Direction direction = Direction.Forward;

            if (np.x < min.x) direction = Direction.Left;
            if (np.x > max.x) direction = Direction.Right;
            if (np.y < min.y) direction = Direction.Back;
            if (np.y > max.y) direction = Direction.Forward;

            return direction;
        }
    }
}