using System;
using Chipmunk.ComponentContainers;
using Code.SHS.Machines.ShapeResources;
using UnityEngine;

namespace Code.SHS.Machines.ResourceVisualizer
{
    public abstract class BaseResourceVisualizer : MonoBehaviour, IExcludeContainerComponent
    {
        protected GameObject resourceObject = null;
        public ComponentContainer ComponentContainer { get; set; }


        public void OnInitialize(ComponentContainer componentContainer)
        {
            gameObject.SetActive(false);
        }


        public virtual void StartTransport(ShapeResource obj)
        {
            CreateObject(obj);

            // resourceObject = Instantiate(obj.ResourceSo.prefab, transform);
            gameObject.SetActive(true);
        }

        protected virtual void CreateObject(ShapeResource obj)
        {
            if (resourceObject != null)
                Destroy(resourceObject);

            // 추후 풀링으로 변경
            resourceObject = new GameObject();
            foreach (ShapePiece shapePiece in obj.ShapePieces)
            {
                if (shapePiece.ShapePieceSo != null)
                    Instantiate(shapePiece.ShapePieceSo.prefab, shapePiece.ShapePieceSo.localPosition,
                        shapePiece.Rotation, resourceObject.transform);
            }

            resourceObject.transform.SetParent(transform, false);
            resourceObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public virtual void EndTransport()
        {
            if (resourceObject != null)
            {
                Destroy(resourceObject);
                resourceObject = null;
            }

            gameObject.SetActive(false);
        }
    }
}