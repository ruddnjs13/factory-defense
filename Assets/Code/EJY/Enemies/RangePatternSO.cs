using UnityEngine;

namespace Code.Patterns.PatternDatas
{
    [CreateAssetMenu(menuName = "SO/Combat/RangePattern")]
    public class RangePatternSO : ScriptableObject
    {
        public float fireAngle = 45f;
        public int bulletCount = 8;
        public float size = 1f;
    }
}