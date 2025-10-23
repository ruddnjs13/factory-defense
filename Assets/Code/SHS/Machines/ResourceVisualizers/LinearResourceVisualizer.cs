using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines.ResourceVisualizers
{
    public class LinearResourceVisualizer : TimerResourceVisualizer
    {
        [SerializeField] private Vector3 _startPoint;
        [SerializeField] private Vector3 _endPoint;


        public override void StartTransport(ShapeResource obj)
        {
            base.StartTransport(obj);
            transform.localPosition = _startPoint;
        }

        protected override void OnProgressChanged(float progress)
        {
            transform.localPosition = Vector3.Lerp(_startPoint, _endPoint, progress);
        }

        private void OnValidate()
        {
            transform.localPosition = _startPoint;
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