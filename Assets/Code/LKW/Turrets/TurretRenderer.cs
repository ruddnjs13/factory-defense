using System;
using System.Collections.Generic;
using Code.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.LKW.Turrets
{
    public class TurretRenderer : MonoBehaviour ,IEntityComponent
    {
        [SerializeField] private ParticleSystem[] damagedSmokeEffects;
        public List<MeshRenderer> renderers;
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void Awake()
        {
            foreach (var effect in damagedSmokeEffects)
            {
                effect.Stop();
            }
        }

        public void ApplyDamagedVisual(float currentHealth, float maxHealth)
        {
            float damagedProgress = (maxHealth - currentHealth) / maxHealth;

            if (damagedProgress >= 0.5f && !damagedSmokeEffects[0].isPlaying)
            {
                foreach (var effect in damagedSmokeEffects)
                {
                    Vector3 randomPosition = transform.position +  Random.insideUnitSphere * 0.4f;
                    randomPosition.y = 0;
                    effect.transform.position = randomPosition;
                    effect.Play();
                }
            }
            
            foreach (var renderer in renderers)
            {

                Color color = Color.Lerp(Color.white, Color.red, damagedProgress * 0.4f);

                renderer.material.color = color;
            }
        }

       
    }
}