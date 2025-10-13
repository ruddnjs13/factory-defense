using Chipmunk.GameEvents;

namespace Code.CoreSystem
{
    public interface IEvent
    {
        void Raise()
        {
            EventBus.Raise(this);
        }
    }
}