using UnityEngine;

namespace Code.SHS.Machines
{
    public interface IMachine : ITick
    {
        public Vector2Int Position { get; }
        public Vector2Int Size { get; }
    }
}