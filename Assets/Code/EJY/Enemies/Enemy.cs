using Code.Entities;
using UnityEngine;

namespace Code.EJY.Enemies
{
    public abstract class Enemy : Entity
    {
        
        public Transform TargetTrm { get; private set; }
        
        public void SetTarget(Transform targetTrm) => TargetTrm = targetTrm;
    }
}