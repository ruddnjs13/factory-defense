using Code.CoreSystem;
using Code.SHS.Machines;
using UnityEngine;

namespace Code.LKW.GameEvents
{
    public struct BuildRequestEvent : IEvent
    {
        public MachineSO BuildingSO { get; set; }

        public Vector3 Position { get; set; }

        public BuildRequestEvent(MachineSO buildingSO, Vector3 position)
        {
            BuildingSO = buildingSO;
            Position = position;
        }
    }
}