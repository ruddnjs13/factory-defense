using UnityEngine;

namespace Code.Combat
{
    [CreateAssetMenu(fileName = "Movement data", menuName = "SO/Combat/Movement", order = 0)]
    public class MovementDataSO : ScriptableObject
    {
        public float maxSpeed;
        public float duration;
        public AnimationCurve movementCurve;
    }
}