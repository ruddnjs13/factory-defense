using Code.SHS.Machines.ShapeResources;

namespace Code.SHS.Machines
{
    public interface IHasResource
    {
        ShapeResource Resource { get; }
    }
}