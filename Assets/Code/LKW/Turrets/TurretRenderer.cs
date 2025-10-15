using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Code.Entities;
using UnityEngine;

namespace Code.LKW.Turrets
{
    public class TurretRenderer : MonoBehaviour ,IEntityComponent
    {
        [SerializeField] private ParticleSystem damagedSmokeEffect;
        public List<MeshRenderer> renderers;
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void Awake()
        {
            damagedSmokeEffect.Stop();
        }

        public void ApplyDamagedVisual(float currentHealth, float maxHealth)
        {
            float damagedProgress = (maxHealth - currentHealth) / maxHealth;

            if (damagedProgress >= 0.5f && damagedSmokeEffect.isPlaying == false)
            {
                damagedSmokeEffect.Play();
            }
            
            foreach (var renderer in renderers)
            {

                Color color = Color.Lerp(Color.white, Color.red, damagedProgress * 0.6f);

                renderer.material.color = color;
            }
        }

       
    }
}