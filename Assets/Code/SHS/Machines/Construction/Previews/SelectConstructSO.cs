using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    [CreateAssetMenu(fileName = "new SelectConstructSO", menuName = "Machine/SelectConstructSO")]
    public class SelectConstructSO : ScriptableObject
    {
        public List<MachineSO> machineSOList;

        [HideInInspector] public List<PortData> outputPorts = new List<PortData>();
        [HideInInspector] public List<PortData> inputPorts = new List<PortData>();

        private void OnEnable()
        {
            outputPorts.Clear();
            inputPorts.Clear();

            foreach (MachineSO so in machineSOList)
            {
                foreach (PortData portData in so.outputPorts)
                {
                    if (!outputPorts.Contains(portData))
                    {
                        outputPorts.Add(portData);
                    }
                }

                foreach (PortData portData in so.inputPorts)
                {
                    if (!inputPorts.Contains(portData))
                    {
                        inputPorts.Add(portData);
                    }
                }
            }
        }
    }
}