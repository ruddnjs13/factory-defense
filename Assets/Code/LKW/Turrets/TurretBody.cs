using System;
using Code.LKW.Building;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.LKW.Turrets
{
    public class TurretBody : MonoBehaviour, ISelectable
    {
        [SerializeField] private TurretBase turret;
        
        ISelectable selectable;

        private void Start()
        {
            selectable = turret.GetComponent<ISelectable>();
        }

        public void Select()
        {
            selectable.Select();
        }

        public void DeSelect()
        {
            if(selectable ==  null) return;
            selectable.DeSelect();
        }
    }
}