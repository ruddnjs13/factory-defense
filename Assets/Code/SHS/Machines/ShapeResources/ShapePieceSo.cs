using System;
using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    [CreateAssetMenu(fileName = "new Resource Piece", menuName = "Resource/Resource Piece", order = 0)]
    public class ResourcePieceSo : ScriptableObject
    {
        public GameObject prefab;
        public Vector3 localPosition;

        private void OnValidate()
        {
            if (prefab != null)
            {
                localPosition = prefab.transform.localPosition;
            }
        }
    }
}