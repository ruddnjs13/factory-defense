using Code.CoreSystem;

namespace Code.SHS.Machines.Events
{
    public struct MachineConstructedEvent : IEvent
    {
        public BaseMachine Machine;
        public MachineConstructedEvent(BaseMachine machine) => Machine = machine;
    }
}