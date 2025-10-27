using Code.Combat;
using UnityEngine;

namespace Code.LKW.Turrets.Combat
{
    public class Bomb : Projectile
    {
        protected override void Update()
        {
            base.Update();
            transform.Rotate(transform.right, -5);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }
    }
}