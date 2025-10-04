using Code.Core.StatSystem;
using Code.Entities;
using Code.Events;
using Code.UI;
using Core.GameEvent;
using UnityEngine;

namespace Code.Combat
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IDamageable, IAfterInitialize
    {
        private Entity _entity;
        private ActionData _actionData;
        private EntityStatCompo _statCompo;
        [SerializeField] private TextInfoSO normal, critical;
        [SerializeField] private GameEventChannelSO textChannel;
        [SerializeField] private StatSO hpStat;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;

        public delegate void OnHealthChanged(float current, float max);
        public event OnHealthChanged onHealthChangedEvent;
        
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _actionData = entity.GetCompo<ActionData>();
            _statCompo = entity.GetCompo<EntityStatCompo>();
        }

        public void AfterInitialize()
        {
            currentHealth = maxHealth = _statCompo.SubscribeStat(hpStat, HandleMaxHPChange, 100f);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(hpStat, HandleMaxHPChange);
        }

        private void HandleMaxHPChange(StatSO stat, float currentValue, float previousValue)
        {
            float changed = currentValue - previousValue;
            maxHealth = currentValue;
            currentHealth = Mathf.Clamp(currentHealth + changed, 0, maxHealth);
        }

        public void ApplyDamage(DamageData damageData, Vector3 hitPoint, Vector3 hitNormal, AttackDataSO attackData,
            Entity dealer)
        {
            _actionData.HitNormal = hitNormal;
            _actionData.HitPoint = hitPoint;
            _actionData.HitByPowerAttack = attackData.isPowerAttack;
            _actionData.DamageData = damageData;

            currentHealth = Mathf.Clamp(currentHealth - damageData.damage, 0, maxHealth);
            
            onHealthChangedEvent?.Invoke(currentHealth, maxHealth);
            
            int typeHash = damageData.isCritical ? critical.nameHash : normal.nameHash;
            Vector3 position = hitPoint + new Vector3(0,1.5f);
            textChannel.RaiseEvent(TextEvents.PopupTextEvent.Init(damageData.damage.ToString(),typeHash,  position, 0.5f));

            if (currentHealth <= 0)
            {
                _entity.OnDeathEvent?.Invoke();
            }

            _entity.OnHitEvent?.Invoke();
        }

       
    }
}