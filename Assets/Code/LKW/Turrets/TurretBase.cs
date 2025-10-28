using System;
using System.Collections.Generic;
using Code.Combat;
using Code.Core.StatSystem;
using Code.EJY.Enemies;
using Code.Entities;
using Code.LKW.ETC;
using Code.SHS.Machines;
using Core.GameEvent;
using RuddnjsPool;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.LKW.Turrets
{
    public abstract class TurretBase : BaseMachine
    {
        [Header("Turret Settings")]
        [Space]
        [SerializeField] protected TurretDataSO turretData;
        [SerializeField] protected AttackDataSO attackData;
        [SerializeField] protected StatSO turretDamageStat;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected GameObject turretHead;
        [SerializeField] protected GameEventChannelSO effectChannel;
        [SerializeField] protected PoolingItemSO muzzleParticleItem;
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private RoundDecal roundDecal;
        [Space]
        [Space]
        
        protected DamageCompo damageCompo;
        protected EntityHealth entityHealth;
        protected TurretRenderer turretRenderer;
        protected EntityStatCompo EntityStatCompo;

        
        private float _reloadTimer;
        protected Enemy _target;
        
        [SerializeField] protected List<Enemy> _targets = new List<Enemy>();

        private bool _isShootAngle = false;

        public int UpgradeIndex { get; set; }
        public int UpgradeCost { get; set; }

        private void Update()
        {
            Tick();
        }

        private void Start()
        {
            UpgradeIndex = turretData.upgradeIndex;
            UpgradeCost = turretData.upgradeCost;
        }

        public virtual void Tick()
        {
            HandleReloadTimer();
            FindTarget();
            RotateShooter();
            TryShoot();
        }

        public override void Select()
        {
            base.Select();
            roundDecal.SetProjectionActive(true);
        }

        public override void DeSelect()
        {
            base.DeSelect();
            roundDecal.SetProjectionActive(false);
        }

        #region Init
        protected override void Awake()
        {
            base.Awake();
            damageCompo = GetCompo<DamageCompo>();
            EntityStatCompo = GetCompo<EntityStatCompo>();
            entityHealth = GetCompo<EntityHealth>();
            turretRenderer = GetCompo<TurretRenderer>();
            
            roundDecal.SetDecalSize(turretData.range / 2);
            roundDecal.SetProjectionActive(false);
        }

        private void OnEnable()
        {
            Debug.Log(turretData);
            sphereCollider.radius = turretData.range;
            entityHealth.onHealthChangedEvent += turretRenderer.ApplyDamagedVisual;
        }

        public override void OnDestroy()
        {
            entityHealth.onHealthChangedEvent -= turretRenderer.ApplyDamagedVisual;
            
            base.OnDestroy();
        }

        #endregion

        #region Shooting Logic
        
        private void HandleReloadTimer()
        {
            if (_reloadTimer > 0f)
                _reloadTimer -= Time.deltaTime;
        }

        protected virtual void TryShoot()
        {
            if (_target == null || _target.IsDead || !_isShootAngle ||_reloadTimer > 0f) return;            

            Shoot();
            _reloadTimer = turretData.reloadTimer;
        }

        protected abstract void Shoot();

        private void RotateShooter()
        {
            if (_target == null || _target.IsDead)
                return;

            Vector3 targetPos = _target.transform.position;
            float distance = Vector3.Distance(transform.position, targetPos);

            if (distance > turretData.range)
            {
                _target = null;
                return;
            }

            Vector3 dir = (targetPos - transform.position).normalized;
            dir.y = 0;

            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0, angle, 0);
            
            float desiredAngle = Mathf.DeltaAngle(turretHead.transform.localEulerAngles.y, angle);
            _isShootAngle = Mathf.Abs(desiredAngle) <= turretData.shootAllowanceAngle;

            turretHead.transform.rotation = Quaternion.RotateTowards(
                turretHead.transform.rotation,
                targetRot,
                turretData.rotationSpeed * Time.deltaTime
            );
        }

        #endregion
        
        #region Targeting Logic
        
        private void FindTarget()
        {
            if (_targets.Count == 0)
            {
                _target = null;
                return;
            }

            Enemy closestTarget = null;
            float closestDistance = float.MaxValue;

            for (int i = _targets.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _targets[i];

                if (enemy == null || enemy.IsDead)
                {
                    _targets.RemoveAt(i);
                    continue;
                }

                float distance = Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.z),
                    new Vector2(enemy.transform.position.x, enemy.transform.position.z)
                );

                if (distance <= turretData.range && distance < closestDistance)
                {
                    closestTarget = enemy;
                    closestDistance = distance;
                }
            }

            _target = closestTarget;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !_targets.Contains(enemy))
            {
                _targets.Add(enemy);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && _targets.Contains(enemy))
            {
                _targets.Remove(enemy);
            }

            if (_target == enemy)
                _target = null;
        }
        #endregion
    }
}