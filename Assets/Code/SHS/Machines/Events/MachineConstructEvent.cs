using Code.CoreSystem;

namespace Code.SHS.Machines.Events
{
    public struct MachineConstructEvent : IEvent
    {
        public BaseMachine Machine;
        public MachineConstructEvent(BaseMachine machine) => Machine = machine;
    }
}