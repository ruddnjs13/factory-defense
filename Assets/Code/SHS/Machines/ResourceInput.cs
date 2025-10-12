using Chipmunk.ComponentContainers;
using UnityEngine;

namespace Code.SHS.Machines
{
    public class ResourceInput : MonoBehaviour, IExcludeContainerComponent
    {
        IInputResource machine;
        public ComponentContainer ComponentContainer { get; set; }

        public void OnInitialize(ComponentContainer componentContainer)
        {
            machine = componentContainer.GetSubclassComponent<IInputResource>();
            Debug.Assert(machine != null, "IResourceInputable component not found in the container.", this);
        }

        public bool CanConnect(ResourceOutput output)
        {
            // 추후 로직 추가
            return true;
        }

        public bool TryInsertResource(Resource resource, DirectionEnum inputDir)
        {
            return true;
        }
    }
}