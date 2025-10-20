using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    [CreateAssetMenu(fileName = "new ResourceSO", menuName = "Resource/ShapeResourceSO", order = 0)]
    public class ShapeResourceSO : ScriptableObject
    {
        [field: SerializeField] public ShapePieceSo[] ShapePieces { get; private set; } = new ShapePieceSo[8];

        public HashSet<ShapePieceSo> ShapePiecesSet { get; private set; } = new HashSet<ShapePieceSo>();

        private void OnEnable()
        {
            ShapePiecesSet = new HashSet<ShapePieceSo>(ShapePieces);
        }

        private void OnValidate()
        {
            if (ShapePieces.Length != 8)
            {
                Debug.LogError(
                    "ResourcePieces array must have exactly 8 elements. \n ResourcePieces 배열은 무조건 8개의 요소가 있어야함;;",
                    this);
            }
        }
    }
}