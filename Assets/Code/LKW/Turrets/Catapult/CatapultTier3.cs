using Code.Combat;
using RuddnjsPool;
using UnityEngine;

namespace Code.LKW.Turrets.Catapult
{
    public class CatapultTier3 : TurretBase
    {
        [SerializeField] private Transform firePos;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        
        private readonly int FIREHash = Animator.StringToHash("FIRE");
        private readonly int RELOADHash = Animator.StringToHash("RELOAD");


        protected override void Awake()
        {
            base.Awake();
            turretRenderer.OnFireTrigger += HandleFireTrigger;
            turretRenderer.OnReloadTrigger += HandleReloadTrigger;
        }

        public override void OnDestroy()
        {
            turretRenderer.OnFireTrigger -= HandleFireTrigger;
            turretRenderer.OnReloadTrigger -= HandleReloadTrigger;
            base.OnDestroy();
        }

        private async void HandleFireTrigger()
        {
            if(_target == null ) return;
            
            Vector3 leftPos = firePos.position + firePos.right * -0.2f;
            Vector3 rightPos = firePos.position + firePos.right * 0.2f;
            
            Vector3 leftVelocity = CalculateVelocity(
                leftPos, 
                _target.transform.position,
                45); 
            Vector3 rightVelocity = CalculateVelocity(
                rightPos, 
                _target.transform.position,
                45);
            
            Projectile bombProjectile = poolManager.Pop(bulletItem.poolType) as Projectile;
            bombProjectile.SetupProjectile(this,damageCompo.CalculateDamage(entityStatCompo.GetStat(turretDamageStat)
                ,attackData),leftPos ,Quaternion.LookRotation(firePos.forward),leftVelocity *  turretData.bulletSpeed);

            await Awaitable.WaitForSecondsAsync(0.02f);
            
            bombProjectile = poolManager.Pop(bulletItem.poolType) as Projectile;
            bombProjectile.SetupProjectile(this,damageCompo.CalculateDamage(entityStatCompo.GetStat(turretDamageStat)
                ,attackData),rightPos ,Quaternion.LookRotation(firePos.forward),rightVelocity *  turretData.bulletSpeed);
        }
        
        

        public Vector3 CalculateVelocity(Vector3 start, Vector3 target, float angleDeg)
        {
            Vector3 velocity = Vector3.zero;

            float g = Mathf.Abs(Physics.gravity.y);

            Vector3 toTarget = target - start;
            Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);

            float x = toTargetXZ.magnitude; // 수평 거리
            float y = toTarget.y;           // 높이 차이

            float angleRad = angleDeg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            float denom = 2 * (x * Mathf.Tan(angleRad) - y);

            float v = Mathf.Sqrt((g * x * x) / (denom * cos * cos));

            Vector3 dirXZ = toTargetXZ.normalized;
            velocity = dirXZ * v * cos + Vector3.up * v * sin;

            return velocity;
        }

        
        private void HandleReloadTrigger()
        {
            turretRenderer.SetParam(RELOADHash);
        }


        protected override void Shoot()
        {
            turretRenderer.SetParam(FIREHash);
        }
    }
}