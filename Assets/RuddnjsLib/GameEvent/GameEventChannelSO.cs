using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameEvent
{
    public abstract class GameEvent
    {
    }

    [CreateAssetMenu(fileName = "GameEventChannel", menuName = "SO/GameEvent/EventChannel", order = 0)]
    public class GameEventChannelSO : ScriptableObject
    {
        private readonly Dictionary<Type, Action<GameEvent>> _events = new Dictionary<Type, Action<GameEvent>>();

        private readonly Dictionary<Delegate, Action<GameEvent>> _lookUpTable =
            new Dictionary<Delegate, Action<GameEvent>>();

        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookUpTable.ContainsKey(handler)) return;

            Action<GameEvent> castHandler = e => handler.Invoke(e as T);
            _lookUpTable[handler] = castHandler;

            var evtType = typeof(T);
            if (!_events.TryAdd(evtType, castHandler))
                _events[evtType] += castHandler;
        }

        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            var evtType = typeof(T);
            if (!_lookUpTable.TryGetValue(handler, out var castHandler)) return;
            if (_events.TryGetValue(evtType, out var internalHandler))
            {
                internalHandler -= castHandler;
                if (internalHandler == null)
                    _events.Remove(evtType);
                else
                    _events[evtType] = internalHandler;
            }

            _lookUpTable.Remove(handler);
        }

        public void RaiseEvent(GameEvent evt)
        {
            if (_events.TryGetValue(evt.GetType(), out var handlers))
                handlers.Invoke(evt);
        }

        public void Clear()
        {
            _events.Clear();
            _lookUpTable.Clear();
        }
    }
}