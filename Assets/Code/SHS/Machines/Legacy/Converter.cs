namespace Code.SHS.Machines.Legacy
{
    public class Converter : ResourceTransporter
    {
        public ResourceSO changeResource;

        public override void OutputItem()
        {
            currentResource = new Resource(changeResource);
            base.OutputItem();
        }
    }
}