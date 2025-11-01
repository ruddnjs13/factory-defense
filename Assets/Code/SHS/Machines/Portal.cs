using System;
using System.Collections.Generic;
using System.Linq;
using Chipmunk.GameEvents;
using Chipmunk.Player.Events;
using Code.Events;
using Code.SHS.Extensions;
using Code.SHS.Machines.Ports;
using Code.SHS.Machines.ShapeResources;
using Code.SHS.Worlds;
using Core.GameEvent;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    public class Portal : BaseMachine, IInputMachine
    {
        [SerializeField] private InputPort[] inputPorts;
        [SerializeField] private List<ShapeResourceSO> allShapeResourceSo;
        [SerializeField] private GameEventChannelSO uiChannel;
        protected override void Awake()
        {
            base.Awake();
            
            // 유니티 이벤트는 알아서 구독해제 해줌
            OnDeathEvent.AddListener(() => uiChannel.RaiseEvent(UIEvents.GameResultEvent.Initializer(false)));
        }

        private void Start()
        {
            WorldGrid.Instance.InstallMachineAt(Position, this);
        }

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

            // We've consumed this resource; return it to the pool to avoid leaking pooled instances
            if (resource != null)
                resource.Push();
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