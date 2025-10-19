using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines.ResourceVisualizer
{
    public abstract class TimerResourceVisualizer : BaseResourceVisualizer
    {
        private float _progress;
        private float _duration;


        void Update()
        {
            if (_progress < 1f)
            {
                _progress += Time.deltaTime / _duration;
                OnProgressChanged(_progress);
            }
        }

        public override void StartTransport(ShapeResource obj)
        {
            base.StartTransport(obj);
            _progress = 0f;
        }

        public virtual void SetDuration(float duration)
        {
            _duration = duration;
        }

        protected abstract void OnProgressChanged(float progress);

        public override void EndTransport()
        {
            base.EndTransport();
            _progress = 1f;
        }
    }
}