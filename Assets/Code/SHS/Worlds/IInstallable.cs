using UnityEngine;

namespace Code.SHS.Worlds
{
    public interface IInstallable
    {
        public Vector2Int Position { get; }
        public Vector2Int Size { get; }
    }
}