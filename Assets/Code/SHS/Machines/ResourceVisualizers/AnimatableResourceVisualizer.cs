using System;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines.ResourceVisualizers
{
    public class AnimatableResourceVisualizer : ResourceVisualizer
    {
        [SerializeField] private Transform[] piecePositions = new Transform[8];
        private readonly GameObject[] pieceObjects = new GameObject[8];

        private void Reset()
        {
            piecePositions = new Transform[8];
            for (int i = 0; i < 8; i++)
            {
                GameObject go = new GameObject($"PiecePosition {i + 1}");
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
                piecePositions[i] = go.transform;
            }
        }

        protected override void CreateObject(ShapeResource obj)
        {
            for (int i = 0; i < 8; i++)
            {
                if (pieceObjects[i] != null)
                {
                    Destroy(pieceObjects[i]);
                    pieceObjects[i] = null;
                }

                if (obj.ShapePieces[i].ShapePieceSo != null)
                {
                    pieceObjects[i] = Instantiate(obj.ShapePieces[i].ShapePieceSo.prefab, piecePositions[i]);
                    // pieceObjects[i].transform.localPosition = obj.ShapePieces[i].ShapePieceSo.localPosition;
                    pieceObjects[i].transform.localRotation = obj.ShapePieces[i].Rotation;
                }
            }
        }
    }
}