using Code.CoreSystem;
using Code.LKW.Building;

namespace Code.LKW.GameEvents
{
    public struct BuildingSelectedEvent : IEvent
    {
        public ISelectable Selectable { get; set; }

        public BuildingSelectedEvent(ISelectable selectable)
        {
            Selectable = selectable;
        }
        
    }
}