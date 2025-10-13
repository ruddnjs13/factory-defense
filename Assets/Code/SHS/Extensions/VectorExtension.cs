using UnityEngine;

namespace Code.SHS.Extensions
{
    public class VectorExtension
    {
    }

    public static class Vector3IntExtensions
    {
        public static Vector2Int ToXZ(this Vector3Int v)
        {
            return new Vector2Int(v.x, v.z);
        }
    }
}