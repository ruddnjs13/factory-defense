using Core.GameEvent;

namespace Code.Events
{
    public static class CameraEvents
    {
        public static ImpulseEvent ImpulseEvent = new ImpulseEvent();
    }

    public class ImpulseEvent : GameEvent
    {
        public float impulsePower;

        public ImpulseEvent Initializer(float power)
        {
            impulsePower = power;
            return this;
        }
    }
}