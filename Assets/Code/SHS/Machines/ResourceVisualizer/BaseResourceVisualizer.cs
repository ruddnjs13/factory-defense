using System;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines.ResourceVisualizer
{
    public abstract class BaseResourceVisualizer : MonoBehaviour, IExcludeContainerComponent
    {
        private float _progress;
        private float _duration;

        private GameObject resourceObject = null;
        public ComponentContainer ComponentContainer { get; set; }


        public void OnInitialize(ComponentContainer componentContainer)
        {
            gameObject.SetActive(false);
        }


        public virtual void StartTransport(ShapeResource obj, float duration)
        {
            _progress = 0f;
            _duration = duration;
            if (resourceObject != null)
                DestroyImmediate(resourceObject);

            // 추후 풀링으로 변경
            resourceObject = new GameObject();
            foreach (var VARIABLE in obj.ResourcePieces)
            {
                if (VARIABLE != null)
                    Instantiate(VARIABLE.prefab, resourceObject.transform);
            }

            resourceObject.transform.SetParent(transform, false);
            // resourceObject = Instantiate(obj.ResourceSo.prefab, transform);
            gameObject.SetActive(true);
        }

        public void EndTransport()
        {
            if (resourceObject != null)
            {
                Destroy(resourceObject);
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