using Code.Combat;
using Code.Sounds;
using RuddnjsPool;
using UnityEngine;

namespace Code.Feedbacks
{
    public class HitSFX : MonoBehaviour
    {
        [field: SerializeField] public DamageType AllowedDamageType;

        [field: SerializeField] public  SoundSO SoundClip { get; private set; }
    }
}