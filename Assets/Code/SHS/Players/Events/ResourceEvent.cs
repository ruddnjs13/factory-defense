using Code.CoreSystem;

namespace Chipmunk.Player.Events
{
    public struct ResourceEvent : IEvent
    {
        public readonly int Amount;

        public ResourceEvent(int amount)
        {
            Amount = amount;
        }
    }
}