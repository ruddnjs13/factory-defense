using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chipmunk.StatSystem
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/StatSystem/Stat", order = 0)]
    public class StatSO : ScriptableObject, ICloneable
    {
        public delegate void ValueChangeHandler(StatSO stat, float current, float previous);

        public event ValueChangeHandler OnValueChange;

        public string statName;
        [TextArea] public string description;

        [SerializeField] public Sprite icon;
        [SerializeField] private float baseValue, minValue, maxValue;

        private Dictionary<object, float> _modifyValueByKey = new Dictionary<object, float>();
        private Dictionary<object, float> _modifyPercentByKey = new Dictionary<object, float>();
        private float _modifiedValue = 0;
        private float _modifiedPercent = 1;

        [field: SerializeField] public bool IsPercent { get; private set; }

        public Sprite Icon => icon;

        [field: SerializeField] public bool NegativeStat { get; private set; } = false;

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

        public float Value => Mathf.Clamp((baseValue + _modifiedValue) * _modifiedPercent, MinValue, MaxValue);
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

        [Obsolete("Use AddValueModifier instead")]
        public void AddModifier(object key, float value) => AddValueModifier(key, value);

        public void AddValueModifier(object key, float value)
        {
            if (_modifyValueByKey.ContainsKey(key)) return;

            float prevValue = Value;
            _modifiedValue += value;
            _modifyValueByKey.Add(key, value);
            TryInvokeValueChangeEvent(Value, prevValue);
        }

        public void RemoveModifier(object key)
        {
            if (_modifyValueByKey.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                _modifiedValue -= value;
                _modifyValueByKey.Remove(key);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        public void AddPercentModifier(object key, float percent)
        {
            Debug.Log("AddPercentModifier: " + key + ", " + percent);
            if (_modifyPercentByKey.ContainsKey(key)) return;

            float prevValue = Value;
            _modifiedPercent *= percent;
            _modifyPercentByKey.Add(key, percent);
            TryInvokeValueChangeEvent(Value, prevValue);
        }

        public void RemovePercentModifier(object key)
        {
            if (_modifyPercentByKey.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                _modifiedPercent /= value;
                _modifyPercentByKey.Remove(key);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        public void ClearModifier()
        {
            float prevValue = Value;
            _modifyValueByKey.Clear();
            _modifiedValue = 0;
            _modifyPercentByKey.Clear();
            _modifiedPercent = 1;
            TryInvokeValueChangeEvent(Value, prevValue);
        }

        private void TryInvokeValueChangeEvent(float value, float prevValue)
        {
            if (Mathf.Approximately(value, prevValue) == false)
            {
                OnValueChange?.Invoke(this, value, prevValue);
            }
        }

        public object Clone()
        {
            return Instantiate(this);
        }
    }
}