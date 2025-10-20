using UnityEngine;

namespace Code.SHS.Worlds
{
    public abstract class GroundSO : ScriptableObject
    {
        [field: SerializeField] public string GroundName { get; private set; }
    }
}