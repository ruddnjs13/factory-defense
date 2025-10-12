namespace Chipmunk.ComponentContainers
{
    public interface IContainerComponent
    {
        public ComponentContainer ComponentContainer { get; set; }

        public void Initialize(ComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
            OnInitialize(componentContainer);
        }

        public void OnInitialize(ComponentContainer componentContainer);
    }
}