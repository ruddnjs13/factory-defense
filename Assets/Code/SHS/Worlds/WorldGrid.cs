using System;
using System.Collections.Generic;
using Chipmunk.GameEvents;
using Code.LKW.GameEvents;
using Code.SHS.Extensions;
using Code.SHS.Machines;
using Code.SHS.Machines.Events;
using UnityEngine;

namespace Code.SHS.Worlds
{
    public class WorldGrid : MonoSingleton<WorldGrid>
    {
        private Dictionary<Vector2Int, GridTile> tiles = new Dictionary<Vector2Int, GridTile>();

        private void Awake()
        {
            EventBus<BuildRequestEvent>.OnEvent += OnBuildRequested;
        }

        private void OnDestroy()
        {
            EventBus<BuildRequestEvent>.OnEvent -= OnBuildRequested;
        }

        private void OnBuildRequested(BuildRequestEvent evt)
        {
            BaseMachine machine = Instantiate(evt.BuildingSO.machinePrefab, evt.Position, Quaternion.identity).GetComponent<BaseMachine>();
            InstallMachineAt(Vector3Int.RoundToInt(evt.Position).ToXZ(), machine);
        }


        public GridTile GetTile(Vector2Int pos)
        {
            if (tiles.TryGetValue(pos, out var tile))
            {
                return tile;
            }

            return default;
        }

        public void SetTile(Vector2Int pos, GridTile tile)
        {
            tiles[pos] = tile;
        }

        public GridTile GetTile(Vector3 pos)
            => GetTile(Vector2Int.RoundToInt(new Vector2(pos.x, pos.z)));

        public void InstallMachineAt(Vector2Int pos, BaseMachine machine)
        {
            for (int x = 0; x < machine.Size.x; x++)
            {
                for (int y = 0; y < machine.Size.y; y++)
                {
                    Vector2Int tilePos = pos + new Vector2Int(x, y) + machine.MachineSo.offset;
                    GridTile tile = GetTile(tilePos);
                    tile.Machine = machine;
                    SetTile(tilePos, tile);
                }
            }

            EventBus.Raise(new MachineConstructedEvent(machine));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (var tile in tiles)
            {
                Vector3 position = new Vector3(tile.Key.x + 0.5f, 0f, tile.Key.y + 0.5f);
                Gizmos.DrawWireCube(position, new Vector3(1f, 0.1f, 1f));
            }
        }
    }
}