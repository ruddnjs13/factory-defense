using System.Collections.Generic;
using Code.LKW.GameEvents;
using Code.SHS.Machines;
using Unity.VisualScripting;
using UnityEngine;
using EventBus = Chipmunk.GameEvents.EventBus;
using Vector3 = System.Numerics.Vector3;

namespace Code.Managers
{
    public enum TurretType
    {
        Default,
        Catapult,
        Missile,
        Flame
    }
    
    public class UpgradeManager : MonoSingleton<UpgradeManager>
    {
        [SerializeField] private List<MachineSO> defaultTurrets;
        [SerializeField] private List<MachineSO> catapultTurrets;
        [SerializeField] private List<MachineSO> missileTurrets;
        [SerializeField] private List<MachineSO> flameTurrets;


        public void Upgrade(TurretType turretType, int upgradeIndex, Vector3 position)
        {
            switch (turretType)
            {
                case TurretType.Default:
                    EventBus.Raise(new BuildRequestEvent(defaultTurrets[upgradeIndex], position));
                    break;
                case TurretType.Catapult:
                    EventBus.Raise(new BuildRequestEvent(catapultTurrets[upgradeIndex], position));
                    break;
                case TurretType.Missile:
                    EventBus.Raise(new BuildRequestEvent(missileTurrets[upgradeIndex], position));
                    break;
                case TurretType.Flame:
                    EventBus.Raise(new BuildRequestEvent(flameTurrets[upgradeIndex], position));
                    break;
            }
        }
    }
}