using System;
using Chipmunk.ComponentContainers;
using UnityEngine;

namespace Code.SHS.Machines.ResourceVisualizer
{
    public abstract class BaseResourceVisualizer : MonoBehaviour, IExcludeContainerComponent
    {
        private float _progress;
        private float _duration;

        private GameObject resourceObject = null;
        public ComponentContainer ComponentContainer { get; set; }

        private ITransporter transporter;

        public void OnInitialize(ComponentContainer componentContainer)
        {
            Debug.Assert(transporter != null,
                $"can not find transporter component in {componentContainer.gameObject.name}");
            gameObject.SetActive(false);
        }


        public virtual void StartTransport(Resource obj, float duration)
        {
            _progress = 0f;
            _duration = duration;
            if (resourceObject != null)
                DestroyImmediate(resourceObject);
            resourceObject = Instantiate(obj.ResourceSo.prefab, transform);
            gameObject.SetActive(true);
        }

        public void EndTransport()
        {
            if (resourceObject != null)
            {
                DestroyImmediate(resourceObject);
                resourceObject = null;
            }

            _progress = 1f;
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (_progress < 1f)
            {
                _progress += Time.deltaTime / _duration;
                OnProgressChanged(_progress);
            }
        }

        protected abstract void OnProgressChanged(float progress);


    }
}