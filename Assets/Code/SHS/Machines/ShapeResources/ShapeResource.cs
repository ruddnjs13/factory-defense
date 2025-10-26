using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    public class ShapeResource
    {
        public ShapePiece[] ShapePieces { get; set; } = new ShapePiece[8];

        private static readonly Stack<ShapeResource> shape_pool = new Stack<ShapeResource>(64);
        private const int MaxPoolSize = 256;

        public static ShapeResource Create(ShapePiece[] ResourcePieces)
        {
            var shapeResource = Pop();

            for (int i = 0; i < 8; i++)
            {
                shapeResource.ShapePieces[i] = (ResourcePieces != null && i < ResourcePieces.Length) ? ResourcePieces[i] : default(ShapePiece);
            }

            return shapeResource;
        }

        public static ShapeResource Create(ShapeResourceSO so)
        {
            var shapeResource = Pop();

            for (int i = 0; i < 8; i++)
            {
                shapeResource.ShapePieces[i] = new ShapePiece(so.ShapePieces[i]);
            }

            return shapeResource;
        }

        private static ShapeResource Pop()
        {
            if (shape_pool.Count > 0)
            {
                return shape_pool.Pop();
            }

            return new ShapeResource();
        }

        public void Push()
        {
            for (int i = 0; i < 8; i++)
            {
                ShapePieces[i] = default(ShapePiece);
            }

            if (shape_pool.Count < MaxPoolSize)
            {
                shape_pool.Push(this);
            }
        }

        private ShapeResource()
        {
            if (ShapePieces == null) ShapePieces = new ShapePiece[8];
        }

        public static ShapeResource Stack(ShapeResource leftResource, ShapeResource rightResource)
        {
            var newResource = Pop();

            for (int i = 0; i < 8; i++)
            {
                if (leftResource != null && leftResource.ShapePieces[i].ShapePieceSo != null)
                {
                    newResource.ShapePieces[i] = leftResource.ShapePieces[i];
                }
                else if (rightResource != null && rightResource.ShapePieces[i].ShapePieceSo != null)
                {
                    newResource.ShapePieces[i] = rightResource.ShapePieces[i];
                }
                else
                {
                    newResource.ShapePieces[i] = default(ShapePiece);
                }
            }

            return newResource;
        }
    }
}