using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.LKW.Turrets.Combat
{
    public class MissileBullet : Projectile
    {
        [SerializeField] private float guidedDelay = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        private Transform _target;

        private Vector3 _defaultDir;
        
        private bool _isFire = false;
        
        protected override void Update()
        {
            base.Update();

            if (_target != null && _isFire)
            {
                if (CurrentLifeTime >= guidedDelay)
                {
                    Vector3 dirToTarget = (_target.position - transform.position).normalized;

                    Quaternion targetRot = Quaternion.LookRotation(dirToTarget);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRot,
                        rotationSpeed * Time.deltaTime * 100f
                    );
                }
            }
            transform.position += transform.forward * (8f * Time.deltaTime);
        }

        public void SetTarget(Transform target, Vector3 defaultDir)
        {
            _target = target;
            _defaultDir = defaultDir.normalized;
        }

        public override void SetupProjectile(Entity owner, DamageData damageData, Vector3 position, Quaternion rotation, Vector3 velocity)
        {
            _isFire = true;
            _owner = owner;
            _damageData = damageData;
            transform.SetPositionAndRotation(position, rotation);
            
            damageCaster.InitCaster(_owner);

            if (isExplosive)
            {
                Debug.Assert(explosionCaster != null, $"Explosion caster is not assigned : {gameObject.name}");
                explosionCaster.InitCaster(_owner);
            }
        }
    }
}