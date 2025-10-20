using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class ConstructPreview : MonoBehaviour
    {
        public MachineSO MachineSO { get; private set; }
        private MachineConstructor constructor;

        public virtual void Initialize(MachineSO machineSO, MachineConstructor constructor)
        {
            MachineSO = machineSO;
            this.constructor = constructor;
        }

        public virtual void SetNextDirection(Direction nextDirection)
        {
            
        }

        public virtual GameObject Construct()
        {
            GameObject machine = Instantiate(MachineSO.machinePrefab, transform.position, transform.rotation);
            return machine;
        }
    }
}