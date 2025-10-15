using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    public class ShapeResource
    {
        [field: SerializeField] public ResourcePieceSo[] ResourcePieces { get; set; } = new ResourcePieceSo[8];

        public static ShapeResource Create(ResourcePieceSo[] ResourcePieces)
        {
            ShapeResource shapeResource = new ShapeResource();
            shapeResource.ResourcePieces = ResourcePieces;
            return shapeResource;
        }

        public static ShapeResource Create(ShapeResourceSO shapeResourceSo)
        {
            ShapeResource shapeResource = new ShapeResource();
            shapeResource.ResourcePieces = shapeResourceSo.ResourcePieces.Clone() as ResourcePieceSo[];
            return shapeResource;
        }

        private ShapeResource()
        {
        }
    }
}