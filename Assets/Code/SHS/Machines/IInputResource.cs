namespace Code.SHS.Machines
{
    public interface IInputResource : IMachine
    {
        public bool CanAcceptInputFrom(IOutputResource machine);
        public bool TryReceiveResource(IOutputResource machine, Resource resource);
        public void ReceiveResource(Resource resource);
        
    }
}