using UnityEngine;

namespace Code.SHS.Machines.ShapeResources
{
    public struct ShapePiece
    {
        public ShapePieceSo ShapePieceSo;
        public Quaternion Rotation;

        public ShapePiece(ShapePieceSo shapePieceSo)
        {
            ShapePieceSo = shapePieceSo;
            Rotation = Quaternion.identity;
        }
    }
}