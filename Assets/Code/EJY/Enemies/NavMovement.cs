using Code.Core.StatSystem;
using Code.EJY.Enemies;
using Code.Entities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemies
{
    public class NavMovement : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float stopOffset = 0.05f; //거리에 대한 오프셋
        [SerializeField] private float rotateSpeed = 10f;
        [field: SerializeField] public int OnUnMovePriority { get; private set; } = 20;
        [field: SerializeField] public int OnMovePriority { get; private set; } = 50;
        
        private Enemy _enemy;
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
            _enemy = entity as Enemy;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            
            SetDestination(_enemy.TargetTrm.position);
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
            _enemy.transform.DOKill();
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
            Vector3 direction = target - _enemy.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            if (isSmooth)
            {
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation,
                    lookRotation, Time.deltaTime * rotateSpeed);
            }
            else
            {
                _enemy.transform.rotation = lookRotation;
            }

            return lookRotation;
        }

        public void SetStop(bool isStop) => agent.isStopped = isStop;
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity;
        public void SetSpeed(float speed) => agent.speed = speed;
        public void SetDestination(Vector3 destination) => agent.SetDestination(destination);
        public void SetPriority(int priority) => agent.avoidancePriority = priority;
    }
}