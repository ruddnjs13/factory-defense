using System;
using UnityEngine;

namespace Blade.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Collider weaponCollider;
        [SerializeField] private new Rigidbody rigidbody;

        private void Awake()
        {
            weaponCollider.enabled = false;
            rigidbody.isKinematic = true;
        }

        public void Drop()
        {
            weaponCollider.enabled = true;
            rigidbody.isKinematic = false;
            transform.SetParent(null);
        }
    }
}