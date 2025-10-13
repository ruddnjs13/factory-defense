using System;
using Chipmunk.ComponentContainers;
using UnityEngine;

namespace Code.SHS.Machines.ResourceVisualizer
{
    public class BaseResourceVisualizer : MonoBehaviour, IExcludeContainerComponent
    {
        private float _progress;
        private float _duration;
        [SerializeField] private Vector3 _startPoint;
        [SerializeField] private Vector3 _endPoint;

        private GameObject resourceObject = null;
        public ComponentContainer ComponentContainer { get; set; }

        private ITransporter transporter;

        public void OnInitialize(ComponentContainer componentContainer)
        {
            Debug.Assert(transporter != null,
                $"can not find transporter component in {componentContainer.gameObject.name}");
            gameObject.SetActive(false);
        }


        private void StartTransport(Resource obj, float duration)
        {
            _progress = 0f;
            _duration = duration;
            resourceObject = Instantiate(obj.ResourceSo.prefab, transform);
            gameObject.SetActive(true);
            transform.localPosition = _startPoint;
        }

        private void EndTransport(Resource obj)
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
                transform.localPosition = Vector3.Lerp(_startPoint, _endPoint, _progress);
            }
        }

        private void OnValidate()
        {
            if (_progress < 1f)
            {
                transform.localPosition = Vector3.Lerp(_startPoint, _endPoint, _progress);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (transform.parent == null) return;

            Gizmos.color = Color.green;
            Vector3 worldStartPosition = transform.parent.TransformPoint(_startPoint);
            Vector3 worldEndPosition = transform.parent.TransformPoint(_endPoint);
            Gizmos.DrawSphere(worldStartPosition, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldEndPosition, 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(worldStartPosition, worldEndPosition);
        }
    }
}