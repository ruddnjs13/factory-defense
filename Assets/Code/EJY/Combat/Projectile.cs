using Code.Entities;
using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;

namespace Code.Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float lifeTime = 15f;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private AttackDataSO attackData;
        [SerializeField] private Rigidbody rbCompo;
        [SerializeField] private PoolingItemSO impactItem;
        [SerializeField] private GameEventChannelSO effectChannel;
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private GameEventChannelSO soundChannel;

        [Space] [Header("Explosion Settings")] [SerializeField]
        private bool isExplosive;

        [SerializeField] private DamageCaster explosionCaster;
        [SerializeField] private SoundSO explosionSound;


        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;

        private Entity _owner;
        private Pool _myPool;
        private DamageData _damageData;
        private float _currentLifeTime = 0;

        public void SetUpPool(Pool pool) => _myPool = pool;

        public void ResetItem()
        {
            _currentLifeTime = 0;
        }

        private void Update()
        {
            _currentLifeTime += Time.deltaTime;
            if(_currentLifeTime >= lifeTime) _myPool.Push(this);
        }

        public void SetupProjectile(Entity owner, DamageData damageData, Vector3 position, Quaternion rotation,
            Vector3 velocity)
        {
            _owner = owner;
            _damageData = damageData;
            transform.SetPositionAndRotation(position, rotation);
            rbCompo.linearVelocity = velocity;

            damageCaster.InitCaster(_owner);

            if (isExplosive)
            {
                Debug.Assert(explosionCaster != null, $"Explosion caster is not assigned : {gameObject.name}");
                explosionCaster.InitCaster(_owner);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            bool isHit = damageCaster.CastDamage(_damageData, transform.position, transform.forward, attackData);

            PlayPoolEffect poolEffectEvt =
                EffectEvents.PlayPoolEffect.Initializer(transform.position, Quaternion.identity, impactItem, 2f);
            effectChannel.RaiseEvent(poolEffectEvt);

            if (isExplosive)
            {
                explosionCaster.CastDamage(_damageData, transform.position, transform.forward, attackData);
                var sfxEvt = SoundsEvents.PlaySfxEvent.Init(transform.position, explosionSound);
                soundChannel.RaiseEvent(sfxEvt);
            }

            if (attackData.impulseForce > 0)
            {
                ImpulseEvent impulseEvt =
                    CameraEvents.ImpulseEvent.Init(attackData.impulseForce);
                cameraChannel.RaiseEvent(impulseEvt);
            }

            _myPool.Push(this); 

            //폭발
        }
    }
}