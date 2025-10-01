using UnityEngine;

namespace Code.LKW.Turrets
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float damage;
    }
}