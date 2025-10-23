using System.Collections.Generic;
using System.Linq;
using Chipmunk.GameEvents;
using Chipmunk.Player.Events;
using Code.SHS.Extensions;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    public class Portal : BaseMachine, IInputMachine
    {
        [SerializeField] private InputPort[] inputPorts;
        [SerializeField] private List<ShapeResourceSO> allShapeResourceSo;

        public InputPort GetAvailableInputPort(OutputPort outputPort)
        {
            foreach (InputPort inputPort in inputPorts)
            {
                if (inputPort.CanAcceptInputFrom(outputPort))
                    return inputPort;
            }

            return null;
        }

        public bool CanAcceptResource()
        {
            return true;
        }

        public void InputPortResourceTransferComplete(InputPort inputPort)
        {
            ShapeResource resource = inputPort.Pop();
            int shapeTier = 0;
            foreach (ShapeResourceSO shapeResourceSo in allShapeResourceSo)
            {
                if (IsContainPiece(resource, shapeResourceSo))
                    shapeTier++;
            }

            int amount = 0;
            for (int i = 0; i < 8; i++)
            {
                if (resource.ShapePieces[i].ShapePieceSo != null)
                    amount += resource.ShapePieces[i].ShapePieceSo.resourceAmount;
            }

            EventBus.Raise(new ResourceEvent(amount * shapeTier));
        }

        private bool IsContainPiece(ShapeResource resource, ShapeResourceSO shapeResourceSo)
        {
            if (resource.ShapePieces.Any(piece => shapeResourceSo.ShapePieces.Contains(piece.ShapePieceSo)))
            {
                return true;
            }

            return false;
        }
    }
}