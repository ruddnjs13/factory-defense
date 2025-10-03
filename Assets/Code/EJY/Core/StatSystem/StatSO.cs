using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blade.Core.StatSystem
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/StatSystem/Stat", order = 0)]
    public class StatSO : ScriptableObject, ICloneable
    {
        public delegate void ValueChangeHandler(StatSO stat, float currentValue, float previousValue);
        
        public event ValueChangeHandler OnValueChanged;

        public string statName;
        public string description;

        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private float baseValue, minValue, maxValue;
        
        private Dictionary<object, float> _modifiedValueByKey = new Dictionary<object, float>();
        [field: SerializeField] public bool IsPercent { get; private set; }
        private float _modifiedValue;
        public Sprite Icon => icon;

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }
        
        public float Value => Mathf.Clamp(baseValue + _modifiedValue, MinValue, MaxValue);
        public bool IsMax => Mathf.Approximately(Value, MaxValue);
        public bool IsMin => Mathf.Approximately(Value, MinValue);

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float prevValue = Value;
                baseValue = Mathf.Clamp(value, MinValue, MaxValue);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        private void TryInvokeValueChangeEvent(float value, float prevValue)
        {
            if(Mathf.Approximately(value, prevValue) == false)
                OnValueChanged?.Invoke(this, value, prevValue);
        }

        public void AddModifier(object key, float value)
        {
            if(_modifiedValueByKey.ContainsKey(key)) return;
            
            float prevValue = Value;
            _modifiedValue += value;
            _modifiedValueByKey.Add(key, value);
            TryInvokeValueChangeEvent(Value, prevValue);
        }

        public void RemoveModifier(object key)
        {
            if (_modifiedValueByKey.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                _modifiedValue -= value;
                _modifiedValueByKey.Remove(key);
                
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        public void ClearModifier()
        {
            float prevValue = Value;
            _modifiedValueByKey.Clear();
            _modifiedValue = 0;
            TryInvokeValueChangeEvent(Value, prevValue);
        }

        public object Clone()
        {
            return Instantiate(this);
        }
    }
}