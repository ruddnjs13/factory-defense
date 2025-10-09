using Code.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.EJY.Enemies
{
    public class TargetDetector : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private float detectRange = 3.5f;

        private Collider[] _hits;
        private Enemy _enemy;

        public bool IsTargeting { get; private set; }
        public Transform CurrentTarget { get; private set; }

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _hits = new Collider[1];
            
        }

        private void FixedUpdate()
        {
            int amount = Physics.OverlapSphereNonAlloc(_enemy.transform.position, detectRange, _hits);
            
            IsTargeting = amount > 0;

            CurrentTarget = IsTargeting ? _hits[0].transform : null;
        }
    }
}