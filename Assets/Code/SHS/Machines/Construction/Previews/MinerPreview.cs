using Code.SHS.Worlds;
using UnityEngine;

namespace Code.SHS.Machines.Construction.Previews
{
    public class MinerPreview : ConstructPreview
    {
        protected override bool CheckCanPlaceAt(Vector2Int tilePos, GridTile tile)
        {
            return tile.Machine == null && tile.Ground is ResourceGroundSO;
        }
    }
}