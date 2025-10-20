using System;
using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    [CreateAssetMenu(fileName = "new Shape Piece", menuName = "Resource/Shape Piece", order = 0)]
    public class ShapePieceSo : ScriptableObject
    {
        public GameObject prefab;
        public Vector3 localPosition;
        public int resourceAmount = 1;
        private void OnValidate()
        {
            if (prefab != null)
            {
                localPosition = prefab.transform.localPosition;
            }
        }
    }
}