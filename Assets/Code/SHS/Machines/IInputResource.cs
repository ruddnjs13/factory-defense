namespace Code.SHS.Machines
{
    public interface IInputResource
    {
        public bool TryInsertResource(Resource resource, DirectionEnum inputDir);
    }
}