using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.Worlds
{
    public class WorldGrid : MonoSingleton<WorldGrid>
    {
        private Dictionary<Vector2Int, WorldTile> tiles = new Dictionary<Vector2Int, WorldTile>();

        public WorldTile GetTile(Vector2Int pos)
        {
            if (tiles.TryGetValue(pos, out var tile))
            {
                return tile;
            }

            return default;
        }
        public void SetTile(Vector2Int pos, WorldTile tile)
        {
            tiles[pos] = tile;
        }

        public WorldTile GetTile(Vector3 pos)
            => GetTile(Vector2Int.RoundToInt(new Vector2(pos.x, pos.z)));
    }
}