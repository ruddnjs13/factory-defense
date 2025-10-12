using Code.CoreSystem;

namespace Code.SHS.Machines.Events
{
    public struct MachineSelectEvent : IEvent
    {
        public MachineSO MachineSo;
        public MachineSelectEvent(MachineSO machineSo)
        {
            MachineSo = machineSo;
        }
    }
}