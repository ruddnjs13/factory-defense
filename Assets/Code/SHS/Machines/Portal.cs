using Code.SHS.Extensions;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using TMPro;
using UnityEngine;

namespace Code.SHS.Machines
{
    public class Portal : BaseMachine, IInputMachine
    {
        [SerializeField] private DirectionEnum facingDirection;
        [SerializeField] private TMP_Text resourceCountText;
        public static int resourceCount = 1000;

        public override void Update()
        {
            base.Update();
            resourceCountText.text = resourceCount.ToString();
        }

        public InputPort GetAvailableInputPort(OutputPort outputPort)
        {
            throw new System.NotImplementedException();
        }

        public bool CanAcceptResource()
        {
            throw new System.NotImplementedException();
        }

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            throw new System.NotImplementedException();
        }
    }
}