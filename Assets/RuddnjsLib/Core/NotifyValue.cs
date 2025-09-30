using System;
using UnityEngine;

[Serializable]
public class NotifyValue<T>
{
    public delegate void ValueChanged(T prev, T next);

    public event ValueChanged OnValueChanged;

    [SerializeField] private T _value;

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            T before = _value;
            _value = value;
            if ((before == null && value != null) || !before.Equals(_value))
                OnValueChanged?.Invoke(before, _value);
        }

    }
    public NotifyValue()
    {
        _value = default(T);
    }

    public NotifyValue(T value)
    {
        _value = value;
    }
}
