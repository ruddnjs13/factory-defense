using Code.SHS.Extensions;
using TMPro;
using UnityEngine;

namespace Code.SHS.Machines
{
    public class Portal : BaseMachine, IInputResource
    {
        [SerializeField] private DirectionEnum facingDirection;
        [SerializeField] private TMP_Text resourceCountText;
        public static int resourceCount = 1000;

        public override void Update()
        {
            base.Update();
            resourceCountText.text = resourceCount.ToString();
        }

        public bool CanAcceptInputFrom(IOutputResource machine)
        {
            Vector2Int direction = Vector3Int.RoundToInt(Direction.GetDirection(facingDirection)).ToXZ();
            Vector2Int anotherDirection = Vector3Int.RoundToInt(Direction.GetDirection(facingDirection)).ToXZ();
            Debug.Log(machine.Position);
            Debug.Log(Position + direction);
            Debug.Log(Position + anotherDirection);
            return machine.Position == Position + direction || machine.Position == Position + anotherDirection;
        }

        public bool TryReceiveResource(IOutputResource machine, Resource resource)
        {
            ReceiveResource(resource);
            return true;
        }

        public void ReceiveResource(Resource resource)
        {
            resourceCount += 1;
        }
    }
}