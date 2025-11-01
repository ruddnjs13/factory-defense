using System.Globalization;
using Chipmunk.ComponentContainers;
using Code.Core.StatSystem;
using Code.Entities;
using Code.Events;
using Code.UI;
using Core.GameEvent;
using UnityEngine;

namespace Code.Combat
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IDamageable, IAfterInitialize, IContainerComponent
    {
        private Entity _entity;
        private ActionData _actionData;
        private EntityStatCompo _statCompo;
        [SerializeField] private StatSO hpStat;
        [SerializeField] private TextInfoSO damageTextInfo;
        [SerializeField] private GameEventChannelSO textChannel;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;

        public delegate void OnHealthChanged(float current, float max);

        public event OnHealthChanged onHealthChangedEvent;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;


        public ComponentContainer ComponentContainer { get; set; }

        public void OnInitialize(ComponentContainer componentContainer)
        {
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _actionData = entity.GetCompo<ActionData>();
            _statCompo = entity.GetCompo<EntityStatCompo>();
        }

        public void Init() => currentHealth = maxHealth;
        
        public void AfterInitialize()
        {
            currentHealth = maxHealth = _statCompo.SubscribeStat(hpStat, HandleMaxHPChange, 100f);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(hpStat, HandleMaxHPChange);
            onHealthChangedEvent?.Invoke(0, maxHealth);
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
            if (_actionData != null)
            {
                _actionData.HitNormal = hitNormal;
                _actionData.HitPoint = hitPoint;
                _actionData.HitByPowerAttack = attackData.isPowerAttack;
                _actionData.DamageData = damageData;
            }

            currentHealth = Mathf.Clamp(currentHealth - damageData.damage, 0, maxHealth);

            if (textChannel != null && damageTextInfo != null)
            {
                Vector3 showPos = hitPoint;
                showPos.y += 5f;
                showPos += Random.onUnitSphere;
                textChannel.RaiseEvent(TextEvents.PopupTextEvent
                    .Initializer(damageData.damage.ToString("#,#"), damageTextInfo.nameHash, showPos, 0.6f));
            }

            onHealthChangedEvent?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                _entity.OnDeathEvent?.Invoke();
            }

            _entity.OnHitEvent?.Invoke();
        }
    }
}