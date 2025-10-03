using Blade.Core;

namespace Blade.Events
{
    public static class CameraEvents
    {
        public static ImpulseEvent ImpulseEvent = new ImpulseEvent();
    }

    public class ImpulseEvent : GameEvent
    {
        public float impulsePower;

        public ImpulseEvent Init(float power)
        {
            impulsePower = power;
            return this;
        }
    }
}