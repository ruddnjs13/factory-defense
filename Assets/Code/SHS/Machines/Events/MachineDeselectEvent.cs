using Code.CoreSystem;

namespace Code.SHS.Machines.Events
{
    public struct MachineDeselectEvent : IEvent
    {
        public MachineSO MachineSo;

        public MachineDeselectEvent(MachineSO machineSo)
        {
            MachineSo = machineSo;
        }
    }
}