using System;
using UnityEngine;

namespace Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public event Action OnAnimationEndTrigger;
        public event Action OnAttackVFXTrigger;
        public event Action<bool> OnManualRotationTrigger;
        public event Action<bool> OnDamageToggleTrigger;
        public event Action OnFireTrigger;
        public event Action OnAttackTrigger;
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
        private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
        private void StartManualRotation() => OnManualRotationTrigger?.Invoke(true);
        private void StopManualRotation() => OnManualRotationTrigger?.Invoke(false);
        private void StartDamageCast() => OnDamageToggleTrigger?.Invoke(true);
        private void StopDamageCast() => OnDamageToggleTrigger?.Invoke(false);
        private void Fire() => OnFireTrigger?.Invoke();
        private void OnAttack() => OnAttackTrigger?.Invoke();
    }
}