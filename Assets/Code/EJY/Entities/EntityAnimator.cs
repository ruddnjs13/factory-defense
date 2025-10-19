using UnityEngine;
using UnityEngine.Events;

namespace Code.Entities
{
    public class EntityAnimator : MonoBehaviour, IEntityComponent
    {
        public UnityEvent<Vector3, Quaternion> OnAnimatorMoveEvent;
        [SerializeField] private Animator animator;

        public bool ApplyRootMotion
        {
            get => animator.applyRootMotion;
            set => animator.applyRootMotion = value;
        }
        
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void OnAnimatorMove()
        {
            // Apply Root motion에 의해서 transform이 움직일 때 호출
            OnAnimatorMoveEvent?.Invoke(animator.deltaPosition, animator.deltaRotation);
        }


        public void SetParam(int hash, float value) => animator.SetFloat(hash, value);
        public void SetParam(int hash, bool value) => animator.SetBool(hash, value);
        public void SetParam(int hash, int value) => animator.SetInteger(hash, value);
        public void SetParam(int hash) => animator.SetTrigger(hash);
        
        public void SetParam(int hash, float value, float dampTime)
        => animator.SetFloat(hash, value, dampTime, Time.deltaTime);

        public void SetAnimator(bool isOn)
        {
            animator.enabled = isOn;
        }
    }
}