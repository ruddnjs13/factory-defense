using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityEvent OnHitEvent;
        public UnityEvent OnDeathEvent;
        
        public bool IsDead { get; set; }
        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            AddComponents();
            InitializeComponents();
            AfterInitializeComponent();
        }

        private void AfterInitializeComponent()
        {
            _components.Values.OfType<IAfterInitialize>().ToList()
                .ForEach(component => component.AfterInitialize());
        }

        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IEntityComponent>().ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        protected virtual void InitializeComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }

        public T GetCompo<T>() where T : class, IEntityComponent
        {
            foreach (var kvp in _components)
            {
                if (typeof(T).IsAssignableFrom(kvp.Key))
                    return kvp.Value as T;
            }

            return null;
        }


        public IEntityComponent GetCompo(Type type)
            => _components.GetValueOrDefault(type);

        public void EntityDestroy()
        {
            Destroy(gameObject);
        }
    }
}