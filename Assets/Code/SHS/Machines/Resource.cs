namespace Code.SHS.Machines
{
    public struct Resource
    {
        public ResourceSO ResourceSo;
        public int Amount;
        public Resource(ResourceSO resourceSo, int amount = 1)
        {
            ResourceSo = resourceSo;
            Amount = amount;
        }
    }
}