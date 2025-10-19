using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class ConstructPreview : MonoBehaviour, IMachine
    {
        public Vector2Int Position { get; private set; }
        public Vector2Int Size { get; }
        public void Initialize(Vector2Int position, DirectionEnum direction)
        {
            Position = position;
        }

        public void OnTick(float deltaTime)
        {
        }
    }
}