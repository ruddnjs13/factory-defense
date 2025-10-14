using System;

namespace Code.SHS.Machines
{
    public interface ITransporter
    {
        float TransportInterval { get; }
        // Action<Resource> OnResourceInput { get; set; }
        Action<Resource> OnResourceOutput { get; set; }
    }
}