using Blade.Core.StatSystem;
using Blade.Entities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Blade.Enemies
{
    public class NavMovement : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float stopOffset = 0.05f; //거리에 대한 오프셋
        [SerializeField] private float rotateSpeed = 10f;
        private Entity _entity;
        private EntityStatCompo _statCompo;
        private Transform _lookAtTrm;

        public bool IsArrived => !agent.pathPending && agent.remainingDistance < agent.stoppingDistance + stopOffset;
        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;
        public Vector3 Velocity => agent.velocity;

        public bool UpdateRotation
        {
            get => agent.updateRotation;
            set => agent.updateRotation = value;
        }

        private float _speedMultiplier = 1f;

        public float SpeedMultiplier
        {
            get => _speedMultiplier;
            set
            {
                _speedMultiplier = value;
                agent.speed = _statCompo.GetStat(moveSpeedStat).Value * _speedMultiplier;
            }
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = _entity.GetCompo<EntityStatCompo>();
        }

        public void AfterInitialize()
        {
            agent.speed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 2f);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentvalue, float previousvalue)
        {
            SetSpeed(currentvalue * _speedMultiplier);
        }

        private void OnDestroy()
        {
            _entity.transform.DOKill();
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
        }

        public void SetLookAtTarget(Transform target)
        {
            _lookAtTrm = target;
            UpdateRotation = target == null;
        }

        private void Update()
        {
            if (_lookAtTrm != null)
            {
                LookAtTarget(_lookAtTrm.position);
            }
            else if (agent.hasPath && agent.isStopped == false && agent.path.corners.Length > 0)
            {
                LookAtTarget(agent.steeringTarget);
            }
        }

        public Quaternion LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = target - _entity.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            if (isSmooth)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
                    lookRotation, Time.deltaTime * rotateSpeed);
            }
            else
            {
                _entity.transform.rotation = lookRotation;
            }

            return lookRotation;
        }

        public void SetStop(bool isStop) => agent.isStopped = isStop;
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity;
        public void SetSpeed(float speed) => agent.speed = speed;
        public void SetDestination(Vector3 destination) => agent.SetDestination(destination);
    }
}