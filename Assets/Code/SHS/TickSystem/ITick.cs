using UnityEngine;

namespace Code.SHS.TickSystem
{
    public interface ITick
    {
        GameObject gameObject { get; }
        void OnTick(float deltaTime);
    }
}