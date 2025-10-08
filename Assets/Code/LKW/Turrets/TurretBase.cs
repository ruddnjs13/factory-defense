using System;
using UnityEngine;

namespace Code.LKW.Turrets
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] private TurretDataSO turretData;
        
        private float _reloadTimer;
       [SerializeField] private Transform _target;

        [SerializeField] private GameObject turretShooter;

        private void Update()
        {
            Tick();
        }

        public virtual void Tick()
        {
            //FindTarget();
            
            RotateShooter();
        }

        public void FindTarget()
        {
            
        }

        public void Shoot()
        {
            
        }
        
        public void RotateShooter()
        {
            if(_target == null) return;
            
            Vector3 dir = (_target.position - transform.position).normalized;
            dir.y = 0;
            
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            
            Quaternion targetRot = Quaternion.Euler(0,angle, 0);

            turretShooter.transform.rotation = Quaternion.RotateTowards
            (
                turretShooter.transform.rotation,
                targetRot,
                turretData.rotationSpeed * Time.deltaTime
            );
        }

        private void OnTriggerEnter(Collider other)
        {
        }
    }
}