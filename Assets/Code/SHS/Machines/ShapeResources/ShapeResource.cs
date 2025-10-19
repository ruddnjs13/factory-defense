using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    public class ShapeResource
    {
        [field: SerializeField] public ShapePiece[] ShapePieces { get; set; } = new ShapePiece[8];

        public static ShapeResource Create(ShapePiece[] ResourcePieces)
        {
            ShapeResource shapeResource = new ShapeResource();
            shapeResource.ShapePieces = ResourcePieces;
            return shapeResource;
        }

        public static ShapeResource Create(ShapeResourceSO so)
        {
            ShapeResource shapeResource = new ShapeResource();

            for (int i = 0; i < 8; i++)
            {
                shapeResource.ShapePieces[i] = new(so.ShapePieces[i]);
                // if (so.ShapePieces[i] != null)
                // {
                //     shapeResource.ShapePieces[i].ShapePieceSo = so.ShapePieces[i];
                // }
                // else
                // {
                //     shapeResource.ShapePieces[i].ShapePieceSo = null;
                // }
            }

            // Debug.LogError("ShapeResource Created");
            return shapeResource;
        }

        private ShapeResource()
        {
        }
    }
}