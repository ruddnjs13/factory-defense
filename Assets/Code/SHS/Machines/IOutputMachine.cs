using Code.SHS.Machines.Ports;
using UnityEngine;

namespace Code.SHS.Machines
{
    public interface IOutputMachine
    {
        void OnOutputPortComplete(OutputPort port);
    }
}