using System;
using Code.SHS.Extensions;
using UnityEngine;

namespace Code.SHS.Worlds
{
    public class MiningField : MonoBehaviour
    {
        [SerializeField] private ResourceGroundSO groundSO;

        private void Awake()
        {
            InitailizeGround();
        }

        private void InitailizeGround()
        {
            Vector2Int gridPosition = Vector3Int.RoundToInt(transform.position).ToXZ();
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector2Int position = gridPosition + new Vector2Int(x, z);
                    WorldTile worldTile = WorldGrid.Instance.GetTile(position);
                    worldTile.Ground = groundSO;
                    WorldGrid.Instance.SetTile(position, worldTile);
                }
            }
        }
    }
}