using UnityEngine;

namespace Code.SHS.Machines
{
    public interface IOutputResource : IMachine
    {
        void OutputItem();
    }
}