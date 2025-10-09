using UnityEngine;

namespace Code.LKW.Turrets
{
    public class DefaultTurret : TurretBase
    {
        protected override void Shoot()
        {
            Debug.Log("shoot");
        }
    }
}