using UnityEngine;

namespace Code.Combat
{
    public class SphereDamageCaster : DamageCaster
    {
        [SerializeField, Range(0.5f, 3f)] private float castRadius = 1f;
        [SerializeField, Range(0, 1f)] private float castInterpolation = 0.5f;
        [SerializeField, Range(0, 3f)] private float castingRange = 1f;
        
        public override bool CastDamage(DamageData damageData, Vector3 position, Vector3 direction, AttackDataSO attackData)
        {
            Vector3 startPos = position + direction * -castInterpolation * 2; //- 붙어있음.
            
            bool isHit = Physics.SphereCast(
                startPos, castRadius, 
                transform.forward, 
                out RaycastHit hit, 
                castingRange,
                whatIsEnemy);

            if (isHit)
            {
                ApplyDamageAndKnockBack(hit.collider.transform, damageData, hit.point, hit.normal, attackData);
            }

            return isHit;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 startPos = transform.position + transform.forward * -castInterpolation * 2;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPos, castRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPos + transform.forward*castingRange, castRadius);
            
        }
#endif
    }
}