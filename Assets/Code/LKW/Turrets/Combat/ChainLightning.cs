using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Entities;
using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using RuddnjsPool;

namespace Code.Combat
{
    public class ChainLightning : Projectile
    {
        [SerializeField] private float chainRadius = 10f;
        [SerializeField] private int maxChains = 4;
        [SerializeField] private float chainDelay = 0.1f;
        [SerializeField] private PoolingItemSO lightningEffectItem;

        private HashSet<Entity> hitTargets = new HashSet<Entity>();

        protected override void OnTriggerEnter(Collider other)
        {
            if (hitTargets.Count > 0) return;

            bool isHit = damageCaster.CastDamage(_damageData, transform.position, transform.forward, attackData);
            Entity firstTarget = other.GetComponent<Entity>();

            if (firstTarget != null)
            {
                hitTargets.Add(firstTarget);
                StartCoroutine(ChainLightningCoroutine(firstTarget.transform, maxChains));
            }

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
                    CameraEvents.ImpulseEvent.Initializer(attackData.impulseForce);
                cameraChannel.RaiseEvent(impulseEvt);
            }

            _myPool.Push(this);
        }

        private IEnumerator ChainLightningCoroutine(Transform startTarget, int remainingChains)
        {
            Transform currentTarget = startTarget;

            for (int i = 0; i < remainingChains; i++)
            {
                yield return new WaitForSeconds(chainDelay);

                Collider[] hits = Physics.OverlapSphere(currentTarget.position, chainRadius);
                Transform nextTarget = null;
                float minDist = Mathf.Infinity;
                Entity nextEntity = null;

                foreach (Collider hit in hits)
                {
                    Entity e = hit.GetComponent<Entity>();
                    if (e != null && !hitTargets.Contains(e) && e != _owner)
                    {
                        float dist = Vector3.Distance(currentTarget.position, hit.transform.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            nextTarget = hit.transform;
                            nextEntity = e;
                        }
                    }
                }

                if (nextTarget == null) yield break;

                hitTargets.Add(nextEntity);
                damageCaster.CastDamage(_damageData, nextTarget.position, Vector3.zero, attackData);

                PlayPoolEffect lightningEffectEvt =
                    EffectEvents.PlayPoolEffect.Initializer(currentTarget.position, Quaternion.identity, lightningEffectItem, 0.2f);
                effectChannel.RaiseEvent(lightningEffectEvt);

                currentTarget = nextTarget;
            }
        }

        public override void SetupProjectile(Entity owner, DamageData damageData, Vector3 position, Quaternion rotation, Vector3 velocity)
        {
            base.SetupProjectile(owner, damageData, position, rotation, velocity);
            hitTargets.Clear();
        }
    }
}
