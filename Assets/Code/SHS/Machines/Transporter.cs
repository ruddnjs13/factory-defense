// using System;
// using System.Collections.Generic;
// using Chipmunk.ComponentContainers;
// using Code.SHS.Extensions;
// using Code.SHS.Machines.Events;
// using Code.SHS.Machines.Ports;
// using Code.SHS.Machines.ResourceVisualizer;
// using Code.SHS.Machines.ShapeResources;
// using Code.SHS.Worlds;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace Code.SHS.Machines
// {
//     public abstract class Transporter : BaseMachine, IInputMachine, IOutputMachine, ITransporter
//     {
//         [SerializeField] protected InputPort inputPort;
//         [SerializeField] protected OutputPort outputPort;
//
//         public ShapeResource currentResource;
//
//         [field: SerializeField] public float TransportInterval { get; private set; } = 1f;
//         private float transferTimer;
//
//         public override void OnInitialize(ComponentContainer componentContainer)
//         {
//             base.OnInitialize(componentContainer);
//
//             float yRotation = transform.eulerAngles.y;
//         }
//
//         protected override void MachineConstructHandler(MachineConstructEvent evt)
//         {
//             base.MachineConstructHandler(evt);
//         }
//
//         public override void OnTick(float deltaTime)
//         {
//             base.OnTick(deltaTime);
//             if (transferTimer >= TransportInterval)
//                 OutputItem();
//             transferTimer += deltaTime;
//         }
//
//         public virtual void OutputItem()
//         {
//             // if (currentResource == null) return;
//             // transferTimer = 0f;
//             // Vector2Int outputPosition =
//             //     Position + Vector3Int.RoundToInt(Direction.GetDirection(outputDirection)).ToXZ();
//             // WorldTile outputTile = WorldGrid.Instance.GetTile(outputPosition);
//             // BaseMachine machine = outputTile.Machine;
//             // if (machine != null && machine is IInputMachine inputMachine)
//             // {
//             //     if (inputMachine.GetAvailableInputPort(outputPort) && currentResource != null)
//             //     {
//             //         if (inputMachine.TryReceiveResource(this, (ShapeResource)currentResource))
//             //         {
//             //             OnResourceOutput?.Invoke((ShapeResource)currentResource);
//             //             resourceVisualizer.EndTransport();
//             //             currentResource = null;
//             //         }
//             //     }
//             // }
//         }
//
//         public InputPort GetAvailableInputPort(OutputPort outputPort)
//         {
//             if (inputPort.CanAcceptInputFrom(outputPort))
//                 return inputPort;
//             return null;
//         }
//
//         public bool CanAcceptResource(ShapeResource shapeResource)
//         {
//             return currentResource == null;
//         }
//
//         public void InputPortResourceTransferComplete(InputPort inputPort)
//         {
//             if (currentResource != null) return;
//             currentResource = this.inputPort.Pop();
//             transferTimer = 0f;
//         }
//     }
// }